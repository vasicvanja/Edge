using Edge.Dtos;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Services.Interfaces
{
    public interface IStripeService
    {
        Task<DataResponse<PaymentDetailsDto>> CreateCheckOutSession(List<ArtworkDto> artworks);
    }
}
