using Edge.Dtos;
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

        private readonly ISmtpSettingsService _smtpSettingsService;
        private readonly IPasswordEncryptionService _passwordEncryptionService;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="smtpSettingsService"></param>
        /// <param name="passwordEncryptionService"></param>
        public EmailService(ISmtpSettingsService smtpSettingsService, IPasswordEncryptionService passwordEncryptionService)
        {
            _smtpSettingsService = smtpSettingsService;
            _passwordEncryptionService = passwordEncryptionService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sends Emails to Users.
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> SendEmail(EmailMessageDto emailMessage)
        {
            var result = new DataResponse<bool> { Data = false, Succeeded = false };

            try
            {
                var smtpSettingsResponse = await _smtpSettingsService.GetSmtpSettings();

                if (!smtpSettingsResponse.Succeeded || smtpSettingsResponse.Data == null)
                {
                    result.Succeeded = false;
                    result.ErrorMessage = ResponseMessages.InvalidInputParameter;
                    return result;
                }

                var smtpSettings = smtpSettingsResponse.Data;

                using (var smtpClient = new SmtpClient(smtpSettings.Host, smtpSettings.Port))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(smtpSettings.Username, _passwordEncryptionService.DecryptPassword(smtpSettings.Password));
                    smtpClient.EnableSsl = smtpSettings.EnableSsl;

                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(smtpSettings.Username);
                        mailMessage.To.Add(new MailAddress(emailMessage.Email));
                        mailMessage.Subject = emailMessage.Subject;
                        mailMessage.Body = emailMessage.Message;
                        mailMessage.IsBodyHtml = emailMessage.IsBodyHtml;

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
