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

namespace Edge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

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
    }
}
