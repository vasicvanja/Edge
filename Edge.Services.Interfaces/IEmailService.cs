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

        /// <summary>
        /// Sends an email to the user with all the successfully purchased artworks.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="artworks"></param>
        /// <returns></returns>
        Task<DataResponse<bool>> SendPurchaseConfirmationEmail(string userEmail, List<ArtworkDto> artworks);
    }
}
