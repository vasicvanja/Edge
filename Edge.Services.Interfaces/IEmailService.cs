using Edge.Dtos;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Services.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends Emails to Users.
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        Task<DataResponse<bool>> SendEmail(EmailMessageDto emailMessage);
    }
}
