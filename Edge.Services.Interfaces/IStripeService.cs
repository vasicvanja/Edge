using Edge.Dtos;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Services.Interfaces
{
    public interface IStripeService
    {
        /// <summary>
        /// Creates a Stripe Checkout Session on a stripe-hosted page.
        /// </summary>
        /// <param name="artworks"></param>
        /// <returns></returns>
        Task<DataResponse<PaymentDetailsDto>> CreateCheckOutSession(List<ArtworkDto> artworks);

        /// <summary>
        /// Retrieve information whether a payment was successful or not.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        Task<DataResponse<bool>> Webhook(Stream body, string signature);
    }
}
