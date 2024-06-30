using Edge.Dtos;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Repositories.Interfaces
{
    public interface ICyclesRepository
    {
        /// <summary>
        /// Get Cycle by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DataResponse<CycleDto>> Get(int Id);

        /// <summary>
        /// Get all Cycles.
        /// </summary>
        /// <returns></returns>
        Task<DataResponse<List<CycleDto>>> GetAll();

        /// <summary>
        /// Create a Cycle with associated Artworks.
        /// </summary>
        /// <param name="createCycleDto"></param>
        /// <returns></returns>
        Task<DataResponse<int>> Create(CreateCycleDto createCycleDto);

        /// <summary>
        /// Update a Cycle.
        /// </summary>
        /// <param name="cycleDto"></param>
        /// <returns></returns>
        Task<DataResponse<bool>> Update(CycleDto cycleDto);

        /// <summary>
        /// Delete a Cycle by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DataResponse<bool>> Delete(int Id);
    }
}
