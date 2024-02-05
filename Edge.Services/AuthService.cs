using Edge.Dtos;
using Edge.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Edge.Services
{
    public class AuthService : IAuthService
    {
        public Task<IdentityResult> Register(RegisterDto registerDto)
        {
            throw new NotImplementedException();
        }

        public Task<string> Login(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

    }
}
