using Edge.Dtos;
using Edge.Repositories.Interfaces;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Services
{
    public class ArtworksService : IArtworksService
    {
        #region Declarations

        private readonly IArtworksRepository _artworkRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="artworkRepository"></param>
        public ArtworksService(IArtworksRepository artworkRepository)
        {
            _artworkRepository = artworkRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get an Artwork by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<DataResponse<ArtworkDto>> Get(int Id) => await _artworkRepository.Get(Id);

        /// <summary>
        /// Get all Artworks.
        /// </summary>
        /// <returns></returns>
        public async Task<DataResponse<List<ArtworkDto>>> GetAll() => await _artworkRepository.GetAll();

        /// <summary>
        /// Get all unassociated Artworks.
        /// </summary>
        /// <returns></returns>
        public async Task<DataResponse<List<ArtworkDto>>> GetAllUnassociatedArtworks() => await _artworkRepository.GetAllUnassociatedArtworks();

        /// <summary>
        /// Get all filtered Artworks.
        /// </summary>
        /// <returns></returns>
        public async Task<DataResponse<List<ArtworkDto>>> GetFilteredArtworks(ArtworkFilterDto filter) => await _artworkRepository.GetFilteredArtworks(filter);

        /// <summary>
        /// Create an Artwork.
        /// </summary>
        /// <param name="artworkDto"></param>
        /// <returns></returns>
        public async Task<DataResponse<int>> Create(ArtworkDto artworkDto) => await _artworkRepository.Create(artworkDto);

        /// <summary>
        /// Update an Artwork.
        /// </summary>
        /// <param name="artworkDto"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> Update(ArtworkDto artworkDto) => await _artworkRepository.Update(artworkDto);

        /// <summary>
        /// Delete an Artwork.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> Delete(int Id) => await _artworkRepository.Delete(Id);

        /// <summary>
        /// Update Artwork in database after payment.
        /// </summary>
        /// <param name="artworks"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> UpdateArtworkQuantity(List<ArtworkDto> artworks) => await _artworkRepository.UpdateArtworkQuantity(artworks);

        #endregion
    }
}
