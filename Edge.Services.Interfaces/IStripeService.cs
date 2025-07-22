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

        /// <summary>
        /// Return all artworks that were a part of a successful payment.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        Task<DataResponse<List<ArtworkDto>>> GetSessionArtworks(string session);
    }
}
