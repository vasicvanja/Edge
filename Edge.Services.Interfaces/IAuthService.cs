using Edge.Dtos;
using Edge.Shared.DataContracts.Responses;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Edge.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> Register(RegisterDto registerDto);
        Task<string> Login(LoginDto loginDto);
        Task Logout();
        Task<DataResponse<bool>> SendForgotPasswordEmail(string email);
        Task<DataResponse<bool>> ResetPassword(ResetPasswordDto resetPassword);
    }
}
