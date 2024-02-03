﻿using Edge.Dtos;
using Edge.Repositories.Interfaces;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Services
{
    public class CyclesService : ICyclesService
    {
        #region Declarations
        private readonly ICyclesRepository _cycleRepository;

        #endregion

        #region Ctor

        public CyclesService(ICyclesRepository cycleRepository)
        {
            _cycleRepository = cycleRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Cycle by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<DataResponse<CycleDto>> Get(int Id) => _cycleRepository.Get(Id);

        /// <summary>
        /// Get all Cycles.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<DataResponse<List<CycleDto>>> GetAll() => _cycleRepository.GetAll();

        /// <summary>
        /// Create a Cycle.
        /// </summary>
        /// <param name="cycleDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<DataResponse<int>> Create(CycleDto cycleDto) => _cycleRepository.Create(cycleDto);

        /// <summary>
        /// Update an Cydl3.
        /// </summary>
        /// <param name="cycleDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<DataResponse<bool>> Update(CycleDto cycleDto) => _cycleRepository.Update(cycleDto);

        /// <summary>
        /// Delete a Cycle.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<DataResponse<bool>> Delete(int Id) => _cycleRepository.Delete(Id);

        #endregion
    }
}
