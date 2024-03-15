using Edge.Dtos;
using Edge.Repositories.Interfaces;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Services
{
    public class SmtpSettingsService : ISmtpSettingsService
    {
        #region Declarations

        private readonly ISmtpSettingsRepository _smtpSettingsRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="smmtpSettingsRepository"></param>
        public SmtpSettingsService(ISmtpSettingsRepository smtpSettingsRepository)
        {
            _smtpSettingsRepository = smtpSettingsRepository;
        }

        #endregion

        #region GET

        /// <summary>
        /// Get all SmtpSetting.
        /// </summary>
        /// <returns></returns>
        public async Task<DataResponse<SmtpSettingsDto>> GetSmtpSettings() => await _smtpSettingsRepository.GetSmtpSettings();

        #endregion

        #region

        /// <summary>
        /// Update an SmtpSetting.
        /// </summary>
        /// <param name="smtpSettingsDto"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> Update(SmtpSettingsDto smtpSettingsDto) => await _smtpSettingsRepository.Update(smtpSettingsDto);

        #endregion
    }
}
