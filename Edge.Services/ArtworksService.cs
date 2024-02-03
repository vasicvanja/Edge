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
        /// <exception cref="NotImplementedException"></exception>
        public Task<DataResponse<ArtworkDto>> Get(int Id) => _artworkRepository.Get(Id);

        /// <summary>
        /// Get all Artworks.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<DataResponse<List<ArtworkDto>>> GetAll() => _artworkRepository.GetAll();

        /// <summary>
        /// Create an Artwork.
        /// </summary>
        /// <param name="artworkDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<DataResponse<int>> Create(ArtworkDto artworkDto) => _artworkRepository.Create(artworkDto);

        /// <summary>
        /// Update an Artwork.
        /// </summary>
        /// <param name="artworkDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<DataResponse<bool>> Update(ArtworkDto artworkDto) => _artworkRepository.Update(artworkDto);

        /// <summary>
        /// Delete an Artwork.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<DataResponse<bool>> Delete(int Id) => _artworkRepository.Delete(Id);

        #endregion
    }
}
