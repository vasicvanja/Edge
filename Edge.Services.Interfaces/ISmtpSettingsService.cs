using Edge.Dtos;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Services.Interfaces
{
    public interface ISmtpSettingsService
    {
        /// <summary>
        /// Get all SmtpSettings.
        /// </summary>
        /// <returns></returns>
        Task<DataResponse<SmtpSettingsDto>> GetSmtpSettings();

        /// <summary>
        /// Update an SmtpSetting.
        /// </summary>
        /// <param name="smtpSettingsDto"></param>
        /// <returns></returns>
        Task<DataResponse<bool>> Update(SmtpSettingsDto smtpSettingsDto);
    }
}
