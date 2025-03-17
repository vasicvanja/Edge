using AutoMapper;
using Edge.Data.EF;
using Edge.DomainModels;
using Edge.Dtos;
using Edge.Repositories.Interfaces;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Resources;
using Edge.Shared.DataContracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Edge.Repositories
{
    public class SmtpSettingsRepository : ISmtpSettingsRepository
    {
        #region Declarations

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly IPasswordEncryptionService _passwordEncryptionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Ctor
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="applicationDbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="passwordEncryptionService"></param>
        /// <param name="httpContextAccessor"></param>
        public SmtpSettingsRepository(ApplicationDbContext applicationDbContext, IMapper mapper, IPasswordEncryptionService passwordEncryptionService, IHttpContextAccessor httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _passwordEncryptionService = passwordEncryptionService;
            _httpContextAccessor = httpContextAccessor;
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
                    var user = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
                    smtpSettingsDto.CreatedBy = user;
                    smtpSettingsDto.ModifiedBy = user;
                    smtpSettingsDto.DateCreated = DateTime.UtcNow;
                    smtpSettingsDto.DateModified = DateTime.UtcNow;

                    var newSmtpSettings = new SmtpSettings();
                    _mapper.Map(smtpSettingsDto, newSmtpSettings);
                    newSmtpSettings.Password = _passwordEncryptionService.EncryptPassword(smtpSettingsDto.Password);
                    _applicationDbContext.SmtpSettings.Add(newSmtpSettings);
                }
                else
                {
                    var user = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
                    smtpSettingsDto.ModifiedBy = user;
                    smtpSettingsDto.DateModified = DateTime.UtcNow;
                    _mapper.Map(smtpSettingsDto, existSmtpSettings);
                    existSmtpSettings.Password = _passwordEncryptionService.EncryptPassword(smtpSettingsDto.Password);
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
