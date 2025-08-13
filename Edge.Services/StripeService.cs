using Edge.Data.EF;
using Edge.DomainModels;
using Edge.Dtos;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Resources;
using Edge.Shared.DataContracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

#pragma warning disable CS8629 // Nullable value type may be null.
#pragma warning disable CS8604 // Possible null reference argument.

namespace Edge.Services
{
    public class StripeService : IStripeService
    {
        #region Declarations

        private readonly StripeSettingsDto _stripeSettings;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IArtworksService _artworksService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly SessionLineItemService _sessionLineItemService;
        private readonly IOrdersService _ordersService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="stripeSettings"></param>
        /// <param name="applicationDbContext"></param>
        /// <param name="artworksService"></param>
        /// <param name="emailService"></param>
        /// <param name="configuration"></param>
        /// <param name="sessionLineItemService"></param>
        public StripeService(
            IOptions<StripeSettingsDto> stripeSettings,
            ApplicationDbContext applicationDbContext,
            IArtworksService artworksService,
            IEmailService emailService,
            IConfiguration configuration,
            SessionLineItemService sessionLineItemService,
            IOrdersService ordersService,
            IHttpContextAccessor httpContextAccessor)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            _applicationDbContext = applicationDbContext;
            _artworksService = artworksService;
            _emailService = emailService;
            _configuration = configuration;
            _sessionLineItemService = sessionLineItemService;
            _ordersService = ordersService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region CHECKOUT SESSION

        /// <summary>
        /// Creates a Stripe Checkout Session on a stripe-hosted page.
        /// </summary>
        /// <param name="artworks"></param>
        /// <returns></returns>
        public async Task<DataResponse<PaymentDetailsDto>> CreateCheckOutSession(List<ArtworkDto> artworks)
        {
            var result = new DataResponse<PaymentDetailsDto> { Data = null, Succeeded = false };

            try
            {
                if (StripeConfiguration.ApiKey == null)
                {
                    result.Succeeded = false;
                    result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                    result.ErrorMessage = string.Format(ResponseMessages.InvalidInputParameter, nameof(StripeConfiguration.ApiKey));
                }

                var lineItems = new List<SessionLineItemOptions>();

                foreach (var artwork in artworks)
                {
                    lineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                //TODO: Pass both the Artwork's Name and Id
                                Name = artwork.Id.ToString()
                            },
                            UnitAmount = (long?)(artwork.Price * 100), // Price in cents
                        },
                        Quantity = artwork.Quantity
                    });
                }

                var clientUrl = _configuration.GetValue<string>("ClientApp:Url");
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                var options = new SessionCreateOptions
                {
                    ClientReferenceId = userId,
                    PaymentMethodTypes = new List<string>
                    {
                        "card"
                    },
                    LineItems = lineItems,
                    Mode = "payment",
                    BillingAddressCollection = "required",
                    SuccessUrl = clientUrl + "/successful-payment?sessionId={CHECKOUT_SESSION_ID}",
                    CancelUrl = clientUrl + "/unsuccessful-payment"
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                var paymentDetails = new PaymentDetailsDto
                {
                    SessionId = session.Id,
                    PaymentStatus = session.PaymentStatus,
                    Status = session.Status
                };

                result.ResponseCode = EDataResponseCode.Success;
                result.Data = paymentDetails;

                return result;
            }
            catch (Exception)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.UnsucessfulCheckoutSessionCreation);

                return result;
            }
        }

        #endregion

        #region WEBHOOK

        /// <summary>
        /// Retrieve information whether a payment was successful or not.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> Webhook(Stream body, string signature)
        {
            var result = new DataResponse<bool> { Data = false, Succeeded = false };

            try
            {
                var json = await new StreamReader(body).ReadToEndAsync();

                if (string.IsNullOrEmpty(json))
                {
                    result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                    result.ErrorMessage = ResponseMessages.EmptyStripeWebhookJson;

                    return result;
                }

                var stripeEvent = EventUtility.ConstructEvent(json, signature, _stripeSettings.WebhookSecret);

                if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var artworks = await GetSessionArtworks(session.Id);
                    await _artworksService.UpdateArtworkQuantity(artworks.Data);

                    var user = session.ClientReferenceId;

                    // If customer is logged in (has an account), create an order record
                    if (!string.IsNullOrEmpty(session.ClientReferenceId))
                    {
                        var totalQuantity = artworks.Data.Sum(a => a.Quantity);
                        var breakdown = string.Join(", ", artworks.Data.Select(a => $"{a.Quantity}x {a.Name}"));

                        var addressParts = new[]
                        {
                            session.CustomerDetails?.Address.Line1,
                            session.CustomerDetails?.Address.Line2,
                            session.CustomerDetails?.Address.City,
                            session.CustomerDetails?.Address.State,
                            session.CustomerDetails?.Address.PostalCode,
                            session.CustomerDetails?.Address.Country
                        };
                        var formattedAddress = string.Join(" ", addressParts.Where(p => !string.IsNullOrWhiteSpace(p)));

                        var orderDto = new OrderDto
                        {
                            UserId = session.ClientReferenceId,
                            Amount = (int)(session.AmountTotal.HasValue ? (decimal)(session.AmountTotal.Value / 100) : 0),
                            Status = session.PaymentStatus,
                            PaymentIntentId = session.PaymentIntentId,
                            ReceiptUrl = session.Invoice?.InvoicePdf ?? session.CustomerDetails?.Email,
                            Description = $"Purchase of {totalQuantity} artwork(s): {breakdown}",
                            BillingAddress = formattedAddress,
                            Metadata = new Dictionary<string, string>
                            {
                                { "ArtworkIds", string.Join(",", artworks.Data.Select(a => a.Id)) },
                                { "SessionId", session.Id }
                            }
                        };

                        await _ordersService.Create(orderDto);
                    }

                    // Send an email to the user with the details of the purchased artworks
                    var userEmail = session.CustomerDetails?.Email;
                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        await _emailService.SendPurchaseConfirmationEmail(userEmail, artworks.Data);
                    }

                    result.Data = true;
                    result.Succeeded = true;
                    result.ResponseCode = EDataResponseCode.Success;
                }
                else
                {
                    result.ResponseCode = EDataResponseCode.CheckoutSessionNotCompleted;
                    result.ErrorMessage = string.Format(ResponseMessages.UnhandledStripeEvent, stripeEvent.Type);
                }

                return result;
            }
            catch (StripeException ex)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = ex.Message;

                return result;
            }
        }

        #endregion

        #region GET SESSION ARTWORKS

        /// <summary>
        /// Return all artworks that were a part of a successful payment.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public async Task<DataResponse<List<ArtworkDto>>> GetSessionArtworks(string sessionId)
        {
            var result = new DataResponse<List<ArtworkDto>> { Data = new List<ArtworkDto>(), Succeeded = false };

            try
            {
                if (sessionId == null)
                {
                    result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                    result.ErrorMessage = ResponseMessages.InvalidInputParameter;

                    return result;
                }

                var lineItems = await _sessionLineItemService.ListAsync(sessionId);

                if (lineItems == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = ResponseMessages.NoDataFound;

                    return result;
                }

                foreach (var lineItem in lineItems.Data)
                {
                    var artworkId = lineItem.Description;

                    var artwork = await _applicationDbContext.Artworks.FindAsync(int.Parse(artworkId));
                    if (artwork != null)
                    {
                        var artworkDto = new ArtworkDto
                        {
                            Id = artwork.Id,
                            Name = artwork.Name,
                            Price = artwork.Price,
                            Quantity = (int)lineItem.Quantity,
                            Description = artwork.Description,
                            ImageData = artwork.ImageData
                        };

                        result.Data.Add(artworkDto);
                    }
                }

                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;

                return result;
            }
            catch (Exception)
            {
                result.Data = null;
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.GettingEntitiesFailed, nameof(Cycle));

                return result;
            }
        }

        #endregion
    }
}
