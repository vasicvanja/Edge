using AutoMapper;
using Edge.Data.EF;
using Edge.DomainModels;
using Edge.Dtos;
using Edge.Repositories.Interfaces;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Resources;
using Edge.Shared.DataContracts.Responses;
using Microsoft.EntityFrameworkCore;

namespace Edge.Repositories
{
    public class ArtworksRepository : IArtworksRepository
    {
        #region Declarations

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        #endregion

        #region Ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationDbContext"></param>
        /// <param name="mapper"></param>
        public ArtworksRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        #endregion

        #region GET 

        /// <summary>
        /// Get Artwork by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<DataResponse<ArtworkDto>> Get(int Id)
        {
            var result = new DataResponse<ArtworkDto> { Data = null, Succeeded = false };

            try
            {
                var artwork = await _applicationDbContext.Artworks.FirstOrDefaultAsync(x => x.Id == Id);

                if (artwork == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NoDataFoundForKey, nameof(Artwork), Id);
                    return result;
                }

                var artworkDto = _mapper.Map<Artwork, ArtworkDto>(artwork);
                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;
                result.Data = artworkDto;

                return result;
            }
            catch (Exception)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.GetEntityFailed, nameof(Artwork), Id);
                return result;
            }
        }

        /// <summary>
        /// Get all Artworks.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DataResponse<List<ArtworkDto>>> GetAll()
        {
            var result = new DataResponse<List<ArtworkDto>> { Data = new List<ArtworkDto>(), Succeeded = false };

            try
            {
                var artworks = await _applicationDbContext.Artworks.ToListAsync();

                if (artworks == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = ResponseMessages.NoDataFound;

                    return result;
                }

                var artworkDto = _mapper.Map<List<Artwork>, List<ArtworkDto>>(artworks);

                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;
                result.Data = artworkDto;

                return result;
            }
            catch (Exception)
            {
                result.Data = null;
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.GettingEntitiesFailed, nameof(Artwork));

                return result;
            }
        }

        #endregion

        #region CREATE

        /// <summary>
        /// Create an Artwork.
        /// </summary>
        /// <param name="artworkDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DataResponse<int>> Create(ArtworkDto artworkDto)
        {
            var result = new DataResponse<int>();

            try
            {
                if (artworkDto == null)
                {
                    result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                    result.ErrorMessage = string.Format(ResponseMessages.InvalidInputParameter, nameof(Artwork));

                    return result;
                }

                var existingArtwork = await _applicationDbContext.Artworks.FirstOrDefaultAsync(x => x.Id == artworkDto.Id);

                if (existingArtwork != null)
                {
                    result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                    result.ErrorMessage = string.Format(ResponseMessages.EntityAlreadyExists, nameof(Artwork), artworkDto.Id);

                    return result;
                }

                var artwork = new Artwork
                {
                    Name = artworkDto.Name,
                    Description = artworkDto.Description,
                    Technique = artworkDto.Technique,
                    Price = artworkDto.Price,
                    Type = artworkDto.Type,
                    Year = artworkDto.Year,
                    ImageData = artworkDto.ImageData,
                    CycleId = artworkDto.CycleId
                };

                await _applicationDbContext.Artworks.AddAsync(artwork);
                await _applicationDbContext.SaveChangesAsync();

                result.Data = artwork.Id;
                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;

                return result;

            }
            catch (Exception)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.UnsuccessfulCreationOfEntity, nameof(Artwork));

                return result;
            }
        }

        #endregion

        #region UPDATE

        /// <summary>
        /// Update an Artwork.
        /// </summary>
        /// <param name="artworkDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DataResponse<bool>> Update(ArtworkDto artworkDto)
        {
            var result = new DataResponse<bool>() { Data = false, Succeeded = false };

            if (artworkDto == null)
            {
                result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                result.ErrorMessage = string.Format(ResponseMessages.InvalidInputParameter, nameof(Artwork));

                return result;
            }

            try
            {
                var existArtwork = await _applicationDbContext.Artworks.FirstOrDefaultAsync(x => x.Id == artworkDto.Id);

                if (existArtwork == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NoDataFoundForKey, nameof(Artwork), artworkDto.Id);

                    return result;
                }

                if (artworkDto.Type != existArtwork.Type)
                {
                    result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                    result.ErrorMessage = ResponseMessages.ChangingArtworkTypeNotAllowed;

                    return result;
                }

                _mapper.Map(artworkDto, existArtwork);

                await _applicationDbContext.SaveChangesAsync();

                result.Data = true;
                result.Succeeded = true;
                result.ResponseCode = EDataResponseCode.Success;

                return result;
            }
            catch (Exception)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.UnsuccessfulUpdateOfEntity, nameof(Artwork));

                return result;
            }
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Delete an Artwork by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DataResponse<bool>> Delete(int Id)
        {
            var result = new DataResponse<bool> { Data = false, Succeeded = false };

            try
            {
                var artwork = await _applicationDbContext.Artworks.FirstOrDefaultAsync(x => x.Id == Id);

                if (artwork == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NoDataFoundForKey, nameof(Artwork), Id);
                    return result;
                }

                _applicationDbContext.Artworks.Remove(artwork);
                await _applicationDbContext.SaveChangesAsync();

                result.Data = true;
                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;

                return result;
            }
            catch (Exception)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.DeletionFailed, nameof(Artwork), Id);

                return result;
            }
        }

        #endregion
    }
}
