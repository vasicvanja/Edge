using Edge.Dtos;
using Edge.Repositories.Interfaces;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Services
{
    public class ContactMessagesService : IContactMessagesService
    {
        #region Declarations

        private readonly IContactMessagesRepository _contactMessagesRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="artworkRepository"></param>
        public ContactMessagesService(IContactMessagesRepository contactMessagesRepository)
        {
            _contactMessagesRepository = contactMessagesRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get a Contact Message by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<DataResponse<ContactMessageDto>> Get(int Id) => await _contactMessagesRepository.Get(Id);

        /// <summary>
        /// Get all Contact Messages.
        /// </summary>
        /// <returns></returns>
        public async Task<DataResponse<List<ContactMessageDto>>> GetAll() => await _contactMessagesRepository.GetAll();

        /// <summary>
        /// Get all Contact Messages by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<DataResponse<List<ContactMessageDto>>> GetAllByEmail(string email) => await _contactMessagesRepository.GetAllByEmail(email);

        /// <summary>
        /// Create a Contact Message.
        /// </summary>
        /// <param name="contactMessageDto"></param>
        /// <returns></returns>
        public async Task<DataResponse<int>> Create(ContactMessageDto contactMessageDto) => await _contactMessagesRepository.Create(contactMessageDto);

        /// <summary>
        /// Delete a Contact Message.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> Delete(int Id) => await _contactMessagesRepository.Delete(Id);

        #endregion
    }
}
