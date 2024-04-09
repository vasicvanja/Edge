using Edge.Dtos;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Services.Interfaces
{
    public interface IArtworksService
    {
        /// <summary>
        /// Get Artwork by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DataResponse<ArtworkDto>> Get(int Id);

        /// <summary>
        /// Get all Artworks.
        /// </summary>
        /// <returns></returns>
        Task<DataResponse<List<ArtworkDto>>> GetAll();

        /// <summary>
        /// Create an Artwork.
        /// </summary>
        /// <param name="artworkDto"></param>
        /// <returns></returns>
        Task<DataResponse<int>> Create(ArtworkDto artworkDto);

        /// <summary>
        /// Update an Artwork.
        /// </summary>
        /// <param name="artworkDto"></param>
        /// <returns></returns>
        Task<DataResponse<bool>> Update(ArtworkDto artworkDto);

        /// <summary>
        /// Delete an Artwork by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DataResponse<bool>> Delete(int Id);

        /// <summary>
        /// Update Artwork in database after payment.
        /// </summary>
        /// <param name="artworkDto"></param>
        /// <returns></returns>
        Task<DataResponse<bool>> UpdateArtworkQuantity(List<ArtworkDto> artworks);
    }
}
