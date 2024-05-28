using Edge.Dtos;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Repositories.Interfaces
{
    public interface IArtworksRepository
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
        /// Update Artworks in database after successful payment.
        /// </summary>
        /// <param name="artworks"></param>
        /// <returns></returns>
        Task<DataResponse<bool>> UpdateArtworkQuantity(List<ArtworkDto> artworks);

        /// <summary>
        /// Delete an Artwork by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DataResponse<bool>> Delete(int Id);
    }
}   
