using Edge.Dtos;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Repositories.Interfaces
{
    public interface ISmtpSettingsRepository
    {
        /// <summary>
        /// Get SmtpSettings by Id.
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
