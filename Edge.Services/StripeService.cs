﻿using Edge.Data.EF;
using Edge.Dtos;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Resources;
using Edge.Shared.DataContracts.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

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
        public StripeService(
            IOptions<StripeSettingsDto> stripeSettings, 
            ApplicationDbContext applicationDbContext, 
            IArtworksService artworksService, 
            IEmailService emailService, 
            IConfiguration configuration)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            _applicationDbContext = applicationDbContext;
            _artworksService = artworksService;
            _emailService = emailService;
            _configuration = configuration;
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

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                    {
                        "card"
                    },
                    LineItems = lineItems,
                    Mode = "payment",
                    SuccessUrl = clientUrl + "/successful-payment",
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

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    List<ArtworkDto> artworks = await GetSessionArtworks(session);
                    await _artworksService.UpdateArtworkQuantity(artworks);

                    // Send an email to the user with the details of the purchased artworks
                    var userEmail = session.CustomerDetails.Email;
                    await _emailService.SendPurchaseConfirmationEmail(userEmail, artworks);

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

        #region Private Methods

        /// <summary>
        /// Return all artworks that were a part of a successful payment.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private async Task<List<ArtworkDto>> GetSessionArtworks(Session session)
        {
            var artworkDtos = new List<ArtworkDto>();

            var service = new SessionService();
            var lineItems = await service.ListLineItemsAsync(session.Id);

            try
            {
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
                            Quantity = (int)lineItem.Quantity
                        };

                        artworkDtos.Add(artworkDto);
                    }
                }

                return artworkDtos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}
