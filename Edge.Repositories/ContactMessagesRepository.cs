using AutoMapper;
using Edge.Data.EF;
using Edge.DomainModels;
using Edge.Dtos;
using Edge.Repositories.Interfaces;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Resources;
using Edge.Shared.DataContracts.Responses;
using Microsoft.EntityFrameworkCore;

namespace Edge.Repositories
{
    public class ContactMessagesRepository : IContactMessagesRepository
    {
        #region Declarations

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        #endregion

        #region Ctor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="applicationDbContext"></param>
        /// <param name="mapper"></param>
        public ContactMessagesRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        #endregion

        #region GET 

        /// <summary>
        /// Get Contact Message by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<DataResponse<ContactMessageDto>> Get(int Id)
        {
            var result = new DataResponse<ContactMessageDto> { Data = null, Succeeded = false };

            try
            {
                var contactMessage = await _applicationDbContext.ContactMessages.FirstOrDefaultAsync(x => x.Id == Id);

                if (contactMessage == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NoDataFoundForKey, nameof(ContactMessage), Id);
                    return result;
                }

                var contactMessageDto = _mapper.Map<ContactMessage, ContactMessageDto>(contactMessage);
                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;
                result.Data = contactMessageDto;

                return result;
            }
            catch (Exception)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.GetEntityFailed, nameof(ContactMessage), Id);
                return result;
            }
        }

        /// <summary>
        /// Get all Contact Messages.
        /// </summary>
        /// <returns></returns>
        public async Task<DataResponse<List<ContactMessageDto>>> GetAll()
        {
            var result = new DataResponse<List<ContactMessageDto>> { Data = new List<ContactMessageDto>(), Succeeded = false };

            try
            {
                var contactMessages = await _applicationDbContext.ContactMessages.ToListAsync();

                if (contactMessages == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = ResponseMessages.NoDataFound;

                    return result;
                }

                var contactMessageDto = _mapper.Map<List<ContactMessage>, List<ContactMessageDto>>(contactMessages);

                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;
                result.Data = contactMessageDto;

                return result;
            }
            catch (Exception)
            {
                result.Data = null;
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.GettingEntitiesFailed, nameof(ContactMessage));

                return result;
            }
        }

        /// <summary>
        /// Get all Contact Messages by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<DataResponse<List<ContactMessageDto>>> GetAllByEmail(string email)
        {
            var result = new DataResponse<List<ContactMessageDto>> { Data = new List<ContactMessageDto>(), Succeeded = false };

            try
            {
                var contactMessages = await _applicationDbContext.ContactMessages
                   .Where(x => x.Email.StartsWith(email) || x.Email.Contains(email))
                   .ToListAsync();


                if (contactMessages == null || contactMessages.Count == 0)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = ResponseMessages.NoDataFound;

                    return result;
                }

                var contactMessageDto = _mapper.Map<List<ContactMessage>, List<ContactMessageDto>>(contactMessages);

                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;
                result.Data = contactMessageDto;

                return result;
            }
            catch (Exception)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.GettingEntitiesFailed, nameof(ContactMessage));

                return result;
            }
        }

        #endregion

        #region CREATE

        /// <summary>
        /// Create a Contact Message.
        /// </summary>
        /// <param name="contactMessageDto"></param>
        /// <returns></returns>
        public async Task<DataResponse<int>> Create(ContactMessageDto contactMessageDto)
        {
            var result = new DataResponse<int>();

            try
            {
                if (contactMessageDto == null)
                {
                    result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                    result.ErrorMessage = string.Format(ResponseMessages.InvalidInputParameter, nameof(ContactMessage));

                    return result;
                }

                var existingContactMessage = await _applicationDbContext.ContactMessages.FirstOrDefaultAsync(x => x.Id == contactMessageDto.Id);

                if (existingContactMessage != null)
                {
                    result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                    result.ErrorMessage = string.Format(ResponseMessages.EntityAlreadyExists, nameof(ContactMessage), contactMessageDto.Id);

                    return result;
                }

                var contactMessage = new ContactMessage
                {
                    Email = contactMessageDto.Email,
                    Phone = contactMessageDto.Phone,
                    Subject = contactMessageDto.Subject,
                    Message = contactMessageDto.Message
                };

                await _applicationDbContext.ContactMessages.AddAsync(contactMessage);
                await _applicationDbContext.SaveChangesAsync();

                result.Data = contactMessage.Id;
                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;

                return result;

            }
            catch (Exception)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.UnsuccessfulCreationOfEntity, nameof(ContactMessage));

                return result;
            }
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Delete an ContactMessage by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> Delete(int Id)
        {
            var result = new DataResponse<bool> { Data = false, Succeeded = false };

            try
            {
                var contactMessage = await _applicationDbContext.ContactMessages.FirstOrDefaultAsync(x => x.Id == Id);

                if (contactMessage == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NoDataFoundForKey, nameof(ContactMessage), Id);
                    return result;
                }

                _applicationDbContext.ContactMessages.Remove(contactMessage);
                await _applicationDbContext.SaveChangesAsync();

                result.Data = true;
                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;

                return result;
            }
            catch (Exception)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.DeletionFailed, nameof(ContactMessage), Id);

                return result;
            }
        }

        #endregion
    }
}