using Edge.Dtos;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Constants;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Resources;
using Edge.Shared.DataContracts.Responses;
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
        private readonly IEmailService _emailService;
        private readonly ISmtpSettingsService _smtpSettingsService;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="configuration"></param>
        /// <param name="emailService"></param>
        /// <param name="smtpSettingsService"></param>
        public AuthService(
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration, 
            IEmailService emailService,
            ISmtpSettingsService smtpSettingsService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailService = emailService;
            _smtpSettingsService = smtpSettingsService;
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

            var smtpSettings = await _smtpSettingsService.GetSmtpSettings();

            if (smtpSettings.Data == null || !smtpSettings.Data.EnableSmtpSettings) return createResult;

            var emailMessage = new EmailMessageDto
            {
                Email = registerDto.Email,
                Subject = "Welcome to Edge",
                Message = "Thank you for registering with us!",
                IsBodyHtml = false
            };

            await _emailService.SendEmail(emailMessage);

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
            var user = await _userManager.FindByNameAsync(loginDto.Username) ?? throw new InvalidOperationException(ResponseMessages.UserDoesNotExist);

            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (passwordCheck)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.UserName!),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

                var signInResult = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
                var generatedToken = string.Empty;

                if (!signInResult.Succeeded) return generatedToken;

                generatedToken = CreateToken(authClaims);
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

        /// <summary>
        /// Sends an email with link to reset password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="resetLink"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> SendForgotPasswordEmail(string email, string resetLink)
        {
            try
            {
                var smtpSettings = await _smtpSettingsService.GetSmtpSettings();

                if (smtpSettings.Data == null)
                {
                    return new DataResponse<bool>
                    {
                        Data = false,
                        ResponseCode = EDataResponseCode.GenericError,
                        ErrorMessage = ResponseMessages.SmtpSettingsNotDefined,
                        Succeeded = false
                    };
                }

                if (smtpSettings.Data != null && !smtpSettings.Data.EnableSmtpSettings)
                {
                    return new DataResponse<bool>
                    {
                        Data = false,
                        ResponseCode = EDataResponseCode.GenericError,
                        ErrorMessage = ResponseMessages.SmtpSettingsDisabled,
                        Succeeded = false
                    };
                }

                var emailMessage = new EmailMessageDto
                {
                    Email = email,
                    Subject = "Password Reset Request",
                    Message = $"Please reset your password by <a href='{resetLink}'> clicking here</a>",
                    IsBodyHtml = true
                };

                var result = await _emailService.SendEmail(emailMessage);

                return result;
            }
            catch (Exception ex)
            {
                return new DataResponse<bool>
                {
                    Data = false,
                    ResponseCode = EDataResponseCode.GenericError,
                    ErrorMessage = ex.Message,
                    Succeeded = false
                };
            }
        }

        /// <summary>
        /// Sets the new password.
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> ResetPassword(ResetPasswordDto resetPassword)
        {
            var result = new DataResponse<bool> { Data = false, Succeeded = false };

            try
            {
                var user = await _userManager.FindByEmailAsync(resetPassword.Email);
                if (user == null)
                {
                    result.Succeeded = false;
                    result.ErrorMessage = ResponseMessages.UserDoesNotExist;
                    result.ResponseCode = EDataResponseCode.GenericError;

                    return result;
                }

                var passwordReset = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);

                if (!passwordReset.Succeeded)
                {
                    result.Succeeded = passwordReset.Succeeded;
                    result.ErrorMessage = ResponseMessages.InvalidToken;
                    result.ResponseCode = EDataResponseCode.InvalidToken;

                    return result;
                }

                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;
                result.Data = passwordReset.Succeeded;

                return result;
            }
            catch (Exception)
            {
                result.Succeeded = false;
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = ResponseMessages.UnsuccessfulPasswordReset;

                return result;
            }
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
        /// Check if user already exists for the provided username and email.
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
