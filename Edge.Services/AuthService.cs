using Edge.Dtos;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Constants;
using Edge.Shared.DataContracts.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace Edge.Services
{
    public class AuthService : IAuthService
    {
        #region Declarations

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        public AuthService(UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Register new user.
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        /// <exception cref="DuplicateNameException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IdentityResult> Register(RegisterDto registerDto)
        {
            var doesUserExist = await CheckIfUserExist(registerDto.Username, registerDto.Email);

            if (doesUserExist)
            {
                throw new DuplicateNameException(ResponseMessages.UserExists);
            }

            IdentityUser newUser = new()
            {
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDto.Username
            };

            var createResult = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(ResponseMessages.UnsuccessfulUserCreation);
            }

            if (await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            else
            {
                throw new InvalidOperationException(ResponseMessages.NonExistingRole);
            }

            return createResult;
        }

        /// <summary>
        /// Sign in existing user.
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="AuthenticationException"></exception>
        public async Task<string> Login(LoginDto loginDto)
        {
            var doesUserExist = await CheckIfUserExist(loginDto.Username);

            if (!doesUserExist)
            {
                throw new InvalidOperationException(ResponseMessages.UserDoesNotExist);
            }

            var user = await _userManager.FindByNameAsync(loginDto.Username);
            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (passwordCheck)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim (ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var signInResult = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
                var generatedToken = string.Empty;

                if (signInResult.Succeeded)
                {
                    generatedToken = CreateToken(authClaims);
                    return generatedToken;
                }

                return generatedToken;
            }
            else
            {
                throw new AuthenticationException(ResponseMessages.InvalidLoginPassword);
            }
        }

        /// <summary>
        /// Sign out user.
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Create JWT Token.
        /// </summary>
        /// <param name="authClaims"></param>
        /// <returns></returns>
        private string CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:SecurityKey"]));
            var credentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = credentials,
                Issuer = _configuration["JWTSettings:ValidIssuer"],
                Audience = _configuration["JWTSettings:ValidAudience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Cheeck if user already exists for the provided username and email.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        private async Task<bool> CheckIfUserExist(string? username = null, string? email = null)
        {
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(email))
            {
                return false;
            }

            var doesEmailExist = !string.IsNullOrEmpty(email) ? await _userManager.FindByEmailAsync(email) : null;
            var doesUsernameExist = !string.IsNullOrEmpty(username) ? await _userManager.FindByNameAsync(username) : null;

            return doesEmailExist != null || doesUsernameExist != null;
        }

        #endregion
    }
}
