using Edge.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Edge.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> Register(RegisterDto registerDto);
        Task<string> Login(LoginDto loginDto);
        Task Logout();
    }
}
