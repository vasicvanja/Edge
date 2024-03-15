using Edge.Dtos;
using Edge.Repositories.Interfaces;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Resources;
using Edge.Shared.DataContracts.Responses;
using System.Net;
using System.Net.Mail;

namespace Edge.Services
{
    public class EmailService : IEmailService
    {
        #region Declarations

        private readonly ISmtpSettingsRepository _smtpSettingsRepository;

        #endregion

        #region Ctor

        public EmailService(ISmtpSettingsRepository smtpSettingsRepository)
        {
            _smtpSettingsRepository = smtpSettingsRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sends Emails to Users.
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<DataResponse<bool>> SendEmail(EmailMessageDto emailMessage)
        {
            var result = new DataResponse<bool> { Data = false, Succeeded = false };

            try
            {
                var smtpSettingsResponse = await _smtpSettingsRepository.GetSmtpSettings();

                if (!smtpSettingsResponse.Succeeded || smtpSettingsResponse.Data == null)
                {
                    result.Succeeded = false;
                    result.ErrorMessage = ResponseMessages.InvalidInputParameter;
                    return result;
                }

                var smtpSettings = smtpSettingsResponse.Data;

                if (smtpSettings.EnableSmtpSettings == false)
                {
                    result.Succeeded = false;
                    result.ErrorMessage = ResponseMessages.SmtpSettingsDisabled;

                    return result;
                }

                using (var smtpClient = new SmtpClient(smtpSettings.Host, smtpSettings.Port))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password);
                    smtpClient.EnableSsl = smtpSettings.EnableSsl;

                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(smtpSettings.Username);
                        mailMessage.To.Add(new MailAddress(emailMessage.Email));
                        mailMessage.Subject = emailMessage.Subject;
                        mailMessage.Body = emailMessage.Message;

                        smtpClient.Send(mailMessage);
                    }
                }

                result.Data = true;
                result.Succeeded = true;
                result.ResponseCode = EDataResponseCode.Success;

                return result;

            }
            catch (Exception) 
            {
                result.Data = false;
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = ResponseMessages.UnsuccessfulEmailSend;

                return result;
            }
        }

        #endregion
    }
}
