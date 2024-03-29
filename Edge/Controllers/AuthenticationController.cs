using Edge.Dtos;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Resources;
using Edge.Shared.DataContracts.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Authentication;
using System.Text.Json;
using System.Web;

namespace Edge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region Declarations

        private readonly IAuthService _authService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="authService"></param>
        public AuthenticationController(IAuthService authService, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _authService = authService;
            _userManager = userManager;
            _configuration = configuration;
        }

        #endregion

        #region Register

        /// <summary>
        /// Register new user.
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseMessages.InvalidModelState);
            }

            try
            {
                var result = await _authService.Register(register);
                return Ok(new DataResponse<IdentityResult>
                {
                    Data = result,
                    Succeeded = true,
                    ErrorMessage = ResponseMessages.SuccessfulUserCreation,
                    ResponseCode = EDataResponseCode.Success
                });
            }
            catch (DuplicateNameException ex)
            {
                var errRet = new DataResponse<RegisterDto>
                {
                    ResponseCode = EDataResponseCode.GenericError,
                    ErrorMessage = ex.Message,
                    Succeeded = false
                };
                return Conflict(errRet);
            }
            catch (InvalidOperationException ex)
            {
                var errRet = new DataResponse<RegisterDto>
                {
                    ResponseCode = EDataResponseCode.GenericError,
                    ErrorMessage = ex.Message,
                    Succeeded = false
                };
                return BadRequest(errRet);
            }
        }

        #endregion

        #region Login

        /// <summary>
        /// Sign in existing user.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            try
            {
                var result = await _authService.Login(login);
                return Ok(new DataResponse<string>
                {
                    Data = result,
                    Succeeded = true,
                    ErrorMessage = ResponseMessages.SuccessfulLogin,
                    ResponseCode = EDataResponseCode.Success
                });
            }
            catch (InvalidOperationException ex)
            {
                var errRet = new DataResponse<LoginDto>
                {
                    ResponseCode = EDataResponseCode.GenericError,
                    ErrorMessage = ex.Message,
                    Succeeded = false
                };
                return NotFound(errRet);
            }
            catch (AuthenticationException ex)
            {
                var errRet = new DataResponse<LoginDto>
                {
                    ResponseCode = EDataResponseCode.GenericError,
                    ErrorMessage = ex.Message,
                    Succeeded = false
                };
                return Unauthorized(errRet);
            }
        }

        #endregion

        #region Logout

        /// <summary>
        /// Logout user.
        /// </summary>
        /// <returns></returns>
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authService.Logout();
                return Ok(JsonSerializer.Serialize(ResponseMessages.SuccessfulUserLogout));
            }
            catch (Exception ex)
            {
                var errRet = new DataResponse<LoginDto>
                {
                    ResponseCode = EDataResponseCode.GenericError,
                    ErrorMessage = ex.Message,
                    Succeeded = false
                };
                return BadRequest(errRet);
            }
        }

        #endregion

        #region Change Password

        /// <summary>
        /// Sends an email with the link for resetting a password.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                var result = await _authService.SendForgotPasswordEmail(email);

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

        /// <summary>
        /// Sets the new password.
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPassword)
        {
            try
            {
                var result = await _authService.ResetPassword(resetPassword);
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
