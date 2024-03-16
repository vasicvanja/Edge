using Edge.Dtos;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Edge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactMessagesController : ControllerBase
    {
        #region Declarations

        private readonly IContactMessagesService _contactMessagesService;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="contactMessagesService"></param>
        public ContactMessagesController(IContactMessagesService contactMessagesService)
        {
            _contactMessagesService = contactMessagesService;
        }

        #endregion

        #region GET

        /// <summary>
        /// Get a Contact Message by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var contactMessage = await _contactMessagesService.Get(id);
                return Ok(Conversion<ContactMessageDto>.ReturnResponse(contactMessage));
            }
            catch (Exception ex)
            {
                var errRet = new DataResponse<ContactMessageDto>
                {
                    Data = null,
                    ResponseCode = EDataResponseCode.GenericError,
                    Succeeded = false,
                    ErrorMessage = ex.Message
                };
                return BadRequest(Conversion<ContactMessageDto>.ReturnResponse(errRet));
            }
        }

        /// <summary>
        /// Get all Contact Messages.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var contactMessages = await _contactMessagesService.GetAll();
                return Ok(Conversion<List<ContactMessageDto>>.ReturnResponse(contactMessages));
            }
            catch (Exception ex)
            {
                var errRet = new DataResponse<List<ContactMessageDto>>
                {
                    Data = null,
                    ResponseCode = EDataResponseCode.GenericError,
                    Succeeded = false,
                    ErrorMessage = ex.Message
                };
                return BadRequest(Conversion<List<ContactMessageDto>>.ReturnResponse(errRet));
            }
        }

        /// <summary>
        /// Get all Contact Messages by email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("allByEmail")]
        public async Task<IActionResult> GetAllByEmail(string email)
        {
            try
            {
                var contactMessages = await _contactMessagesService.GetAllByEmail(email);
                return Ok(Conversion<List<ContactMessageDto>>.ReturnResponse(contactMessages));
            }
            catch (Exception ex)
            {
                var errRet = new DataResponse<List<ContactMessageDto>>
                {
                    Data = null,
                    ResponseCode = EDataResponseCode.GenericError,
                    Succeeded = false,
                    ErrorMessage = ex.Message
                };
                return BadRequest(Conversion<List<ContactMessageDto>>.ReturnResponse(errRet));
            }
        }

        #endregion

        #region CREATE

        /// <summary>
        /// Create new Contact Message.
        /// </summary>
        /// <param name="contactMessage"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(ContactMessageDto contactMessage)
        {
            try
            {
                var result = await _contactMessagesService.Create(contactMessage);
                return Ok(Conversion<int>.ReturnResponse(result));
            }
            catch (Exception ex)

            {
                var errRet = new DataResponse<int>
                {
                    ResponseCode = EDataResponseCode.GenericError,
                    Succeeded = false,
                    ErrorMessage = ex.Message
                };
                return BadRequest(Conversion<int>.ReturnResponse(errRet));
            }
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Delete an Artwork by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _contactMessagesService.Delete(id);
                return Ok(Conversion<bool>.ReturnResponse(result));
            }
            catch (Exception ex)
            {
                var errRet = new DataResponse<bool>
                {
                    ResponseCode = EDataResponseCode.GenericError,
                    Succeeded = false,
                    ErrorMessage = ex.Message
                };
                return BadRequest(Conversion<bool>.ReturnResponse(errRet));
            }
        }

        #endregion
    }
}
