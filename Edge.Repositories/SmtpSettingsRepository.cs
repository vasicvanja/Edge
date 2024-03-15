﻿using AutoMapper;
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
    public class SmtpSettingsRepository : ISmtpSettingsRepository
    {
        #region Declarations

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        #endregion

        #region Ctor
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="applicationDbContext"></param>
        public SmtpSettingsRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        #endregion

        #region GET

        /// <summary>
        /// Get all SmtpSettings.
        /// </summary>
        /// <returns></returns>
        public async Task<DataResponse<SmtpSettingsDto>> GetSmtpSettings()
        {
            var result = new DataResponse<SmtpSettingsDto> { Data = null, Succeeded = false };

            try
            {
                var smtpSettings = await _applicationDbContext.SmtpSettings.FirstOrDefaultAsync();

                if (smtpSettings == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = ResponseMessages.NoDataFound;

                    return result;
                }

                var smtpSettingsDto = _mapper.Map<SmtpSettings, SmtpSettingsDto>(smtpSettings);

                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;
                result.Data = smtpSettingsDto;

                return result;
            }
            catch (Exception)
            {
                result.Data = null;
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.GettingEntitiesFailed, nameof(SmtpSettings));

                return result;
            }
        }

        #endregion

        #region UPDATE

        /// <summary>
        /// Update an SmtpSetting.
        /// </summary>
        /// <param name="smtpSettingsDto"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> Update(SmtpSettingsDto smtpSettingsDto)
        {
            var result = new DataResponse<bool>() { Data = false, Succeeded = false };

            if (smtpSettingsDto == null)
            {
                result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                result.ErrorMessage = string.Format(ResponseMessages.InvalidInputParameter, nameof(SmtpSettings));

                return result;
            }

            try
            {
                var existSmtpSettings = await _applicationDbContext.SmtpSettings.FirstOrDefaultAsync(x => x.Id == smtpSettingsDto.Id);

                if (existSmtpSettings == null)
                {
                    var newSmtpSettings = new SmtpSettings();
                    _mapper.Map(smtpSettingsDto, newSmtpSettings);
                    _applicationDbContext.SmtpSettings.Add(newSmtpSettings);
                }
                else
                {
                    _mapper.Map(smtpSettingsDto, existSmtpSettings);
                }

                await _applicationDbContext.SaveChangesAsync();

                result.Data = true;
                result.Succeeded = true;
                result.ResponseCode = EDataResponseCode.Success;

                return result;
            }
            catch (Exception)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.UnsuccessfulUpdateOfEntity, nameof(SmtpSettings));

                return result;
            }   
        }

        #endregion
    }
}
