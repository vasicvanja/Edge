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
    public class CyclesRepository : ICyclesRepository
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
        public CyclesRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        #endregion

        #region GET 

        /// <summary>
        /// Get Cycle by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<DataResponse<CycleDto>> Get(int Id)
        {
            var result = new DataResponse<CycleDto> { Data = null, Succeeded = false };

            try
            {
                var cycle = await _applicationDbContext
                    .Cycles
                    .Include(c => c.Artworks)
                    .FirstOrDefaultAsync(x => x.Id == Id);

                if (cycle == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NoDataFoundForKey, nameof(Cycle), Id);
                    return result;
                }

                var cycleDto = _mapper.Map<Cycle, CycleDto>(cycle);
                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;
                result.Data = cycleDto;

                return result;
            }
            catch (Exception)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.GetEntityFailed, nameof(Cycle), Id);
                return result;
            }
        }

        /// <summary>
        /// Get all Cycles.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DataResponse<List<CycleDto>>> GetAll()
        {
            var result = new DataResponse<List<CycleDto>> { Data = new List<CycleDto>(), Succeeded = false };

            try
            {
                var cycles = await _applicationDbContext.Cycles.ToListAsync();

                if (cycles == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = ResponseMessages.NoDataFound;

                    return result;
                }

                var cycleDto = _mapper.Map<List<Cycle>, List<CycleDto>>(cycles);

                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;
                result.Data = cycleDto;

                return result;
            }
            catch (Exception)
            {
                result.Data = null;
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.GettingEntitiesFailed, nameof(Cycle));

                return result;
            }
        }

        #endregion

        #region CREATE

        /// <summary>
        /// Create a Cycle.
        /// </summary>
        /// <param name="cycleDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DataResponse<int>> Create(CycleDto cycleDto)
        {
            var result = new DataResponse<int>();

            try
            {
                if (cycleDto == null)
                {
                    result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                    result.ErrorMessage = string.Format(ResponseMessages.InvalidInputParameter, nameof(Cycle));

                    return result;
                }

                var existingCycle = await _applicationDbContext.Cycles.FirstOrDefaultAsync(x => x.Id == cycleDto.Id);

                if (existingCycle != null)
                {
                    result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                    result.ErrorMessage = string.Format(ResponseMessages.EntityAlreadyExists, nameof(Cycle), cycleDto.Id);

                    return result;
                }

                var cycle = new Cycle
                {
                    Name = cycleDto.Name,
                    Description = cycleDto.Description,
                    Artworks = _mapper.Map<List<Artwork>>(cycleDto.Artworks)
                };

                await _applicationDbContext.Cycles.AddAsync(cycle);
                await _applicationDbContext.SaveChangesAsync();

                result.Data = cycle.Id;
                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;

                return result;

            }
            catch (Exception)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.UnsuccessfulCreationOfEntity, nameof(Cycle));

                return result;
            }
        }

        #endregion

        #region UPDATE

        /// <summary>
        /// Update a Cycle.
        /// </summary>
        /// <param name="cycleDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DataResponse<bool>> Update(CycleDto cycleDto)
        {
            var result = new DataResponse<bool>() { Data = false, Succeeded = false };

            if (cycleDto == null)
            {
                result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                result.ErrorMessage = string.Format(ResponseMessages.InvalidInputParameter, nameof(Cycle));

                return result;
            }

            try
            {
                var existingCycle = await _applicationDbContext.Cycles.FirstOrDefaultAsync(x => x.Id == cycleDto.Id);

                if (existingCycle == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NoDataFoundForKey, nameof(Cycle), cycleDto.Id);

                    return result;
                }

                _mapper.Map(cycleDto, existingCycle);

                await _applicationDbContext.SaveChangesAsync();

                result.Data = true;
                result.Succeeded = true;
                result.ResponseCode = EDataResponseCode.Success;

                return result;
            }
            catch (Exception)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.UnsuccessfulUpdateOfEntity, nameof(Cycle));

                return result;
            }
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Delete a Cycle by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DataResponse<bool>> Delete(int Id)
        {
            var result = new DataResponse<bool> { Data = false, Succeeded = false };

            try
            {
                var cycle = await _applicationDbContext.Cycles.FirstOrDefaultAsync(x => x.Id == Id);

                if (cycle == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NoDataFoundForKey, nameof(Cycle), Id);
                    return result;
                }

                _applicationDbContext.Cycles.Remove(cycle);
                await _applicationDbContext.SaveChangesAsync();

                result.Data = true;
                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;

                return result;
            }
            catch (Exception)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.DeletionFailed, nameof(Cycle), Id);

                return result;
            }
        }

        #endregion
    }
}
