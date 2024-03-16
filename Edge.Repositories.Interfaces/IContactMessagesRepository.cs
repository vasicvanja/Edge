using Edge.Dtos;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Repositories.Interfaces
{
    public interface IContactMessagesRepository
    {
        /// <summary>
        /// Get Contact Message by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DataResponse<ContactMessageDto>> Get(int Id);

        /// <summary>
        /// Get all Contact Messages.
        /// </summary>
        /// <returns></returns>
        Task<DataResponse<List<ContactMessageDto>>> GetAll();

        /// <summary>
        /// Get all Contact Messages by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<DataResponse<List<ContactMessageDto>>> GetAllByEmail(string email);

        /// <summary>
        /// Create a Contact Message.
        /// </summary>
        /// <param name="contactMessageDto"></param>
        /// <returns></returns>
        Task<DataResponse<int>> Create(ContactMessageDto contactMessageDto);

        /// <summary>
        /// Delete a Contact Message by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DataResponse<bool>> Delete(int Id);
    }
}
