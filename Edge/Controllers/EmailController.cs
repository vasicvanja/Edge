using Edge.Dtos;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Edge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        #region Declarations

        private readonly IEmailService _emailService;

        #endregion

        #region Ctor
        
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="emailService"></param>
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        #endregion

        #region Methods

        [HttpPost]
        [Route("sendEmail")]
        public async Task<IActionResult> SendEmail(EmailMessageDto emailMessage)
        {
            try
            {
                var result = await _emailService.SendEmail(emailMessage);
                return Ok(Conversion<bool>.ReturnResponse(result));
            }
            catch (Exception ex)
            {
                var errRet = new DataResponse<bool>
                {
                    ResponseCode = EDataResponseCode.Success,
                    Succeeded = false,
                    ErrorMessage = ex.Message
                };
                return BadRequest(Conversion<bool>.ReturnResponse(errRet));
            }
        }

        #endregion
    }
}
