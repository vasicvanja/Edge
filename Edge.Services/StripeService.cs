using Edge.DomainModels;
using Edge.Dtos;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Resources;
using Edge.Shared.DataContracts.Responses;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace Edge.Services
{
    public class StripeService : IStripeService
    {
        #region Declarations

        private readonly StripeSettingsDto _stripeSettings;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        public StripeService(IOptions<StripeSettingsDto> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create Stripe checkout session.
        /// </summary>
        /// <returns></returns>
        public async Task<DataResponse<PaymentDetailsDto>> CreateCheckOutSession(List<ArtworkDto> artworks)
        {
            var result = new DataResponse<PaymentDetailsDto> { Data = null, Succeeded = false };

            try
            {
                StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

                if (StripeConfiguration.ApiKey == null)
                {
                    result.Succeeded = false;
                    result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                    result.ErrorMessage = string.Format(ResponseMessages.InvalidInputParameter, nameof(StripeConfiguration.ApiKey));
                }

                var lineItems = new List<SessionLineItemOptions>();

                foreach(var artwork in artworks)
                {
                    lineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = artwork.Name,
                            },
                            UnitAmount = (long?)(artwork.Price * 100), // Price in cents
                        },
                        Quantity = artwork.Quantity,
                    });
                }

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                    {
                        "card"
                    },
                    LineItems = lineItems,
                    Mode = "payment",
                    SuccessUrl = "https://example.com/success",
                    CancelUrl = "https://example.com/cancel",
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
    }
}
