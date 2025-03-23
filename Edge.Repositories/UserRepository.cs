using CustomValidation.Interface;
using Edge.Data.EF;
using Edge.Dtos;
using Edge.Repositories.Interfaces;
using Edge.Shared.DataContracts.Constants;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Resources;
using Edge.Shared.DataContracts.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Edge.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        #region Declarations

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailValidator _emailValidator;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="userManager"></param>
        public UsersRepository(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager, IEmailValidator emailValidator)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _emailValidator = emailValidator;
        }

        #endregion

        #region GET

        /// <summary>
        /// Get User by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DataResponse<UserDto>> Get(string id)
        {
            var result = new DataResponse<UserDto> { Data = null, Succeeded = false };

            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NoDataFoundForKey, "User", id);

                    return result;
                }

                var roles = await _userManager.GetRolesAsync(user);

                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = roles[0]
                };

                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;
                result.Data = userDto;

                return result;
            }
            catch (Exception)
            {
                result.Data = null;
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.GetEntityFailed, "User", id);

                return result;
            }
        }

        /// <summary>
        /// Get all Users.
        /// </summary>
        /// <returns></returns>
        public async Task<DataResponse<List<UserDto>>> GetAll()
        {
            var result = new DataResponse<List<UserDto>> { Data = null, Succeeded = false };

            try
            {
                var users = await _userManager.Users.ToListAsync();

                if (!users.Any())
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = ResponseMessages.NoDataFound;

                    return result;
                }

                var userDtos = new List<UserDto>();

                foreach (var user in users)
                {
                    var role = await _userManager.GetRolesAsync(user);
                    userDtos.Add(new UserDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Role = role[0]
                    });
                }

                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;
                result.Data = userDtos;

                return result;
            }
            catch (Exception)
            {
                result.Data = null;
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.GettingEntitiesFailed, "User");

                return result;
            }
        }

        #endregion

        #region CREATE

        /// <summary>
        /// Crate a User.
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <returns></returns>
        public async Task<DataResponse<string>> Create(CreateUserDto user)
        {
            var result = new DataResponse<string> { Data = null, Succeeded = false };

            await using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();

            try
            {
                //If user Email value is empty and Username value is a valid email address, set the Email 
                if (string.IsNullOrEmpty(user.Email) && _emailValidator.IsValidEmail(user.UserName))
                {
                    user.Email = user.UserName;
                }

                // Check if the username is already used
                var userNameUsed = await _userManager.Users.AnyAsync(x => x.NormalizedUserName == user.UserName.ToUpperInvariant());
                if (userNameUsed)
                {
                    result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                    result.ErrorMessage = string.Format(ResponseMessages.UsernameAlreadyTaken, userNameUsed);
                    return result;
                }

                // Check if the username is used as an email for another user
                var userNameUsedAsEmail = await _userManager.Users.AnyAsync(x => x.NormalizedEmail == user.UserName.ToUpperInvariant());
                if (userNameUsedAsEmail)
                {
                    result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                    result.ErrorMessage = string.Format(ResponseMessages.UsernameAlreadyTakenAsEmailFromOtherUser, user.UserName);
                    return result;
                }

                // Check if the email is already used
                if (!string.IsNullOrEmpty(user.Email))
                {
                    var userEmailUsed = await _userManager.Users.AnyAsync(x => x.NormalizedEmail == user.Email.ToUpperInvariant());

                    if (userEmailUsed)
                    {
                        result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                        result.ErrorMessage = string.Format(ResponseMessages.EmailAlreadyExists, user.Email);

                        return result;
                    }

                    // Check if the email is used as a username
                    var userEmailUsedAsUsername = await _userManager.Users.AnyAsync(x => x.NormalizedUserName == user.Email.ToUpperInvariant());
                    if (userEmailUsedAsUsername)
                    {
                        result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                        result.ErrorMessage = string.Format(ResponseMessages.EmailTakenAsUsernameFromOtherUser, user.Email);

                        return result;
                    }
                }

                //Map CreateUserDto to IdentityUser
                var identityUser = new IdentityUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    NormalizedUserName = user.UserName.ToUpperInvariant(),
                    NormalizedEmail = user.Email.ToUpperInvariant(),
                    PhoneNumber = user.PhoneNumber
                };

                // Create user
                var createUser = await _userManager.CreateAsync(identityUser, user.Password);

                if (!createUser.Succeeded)
                {
                    await transaction.RollbackAsync();

                    result.ResponseCode = EDataResponseCode.GenericError;
                    result.ErrorMessage = string.Format(ResponseMessages.UnsuccessfulCreationOfEntity, "User");
                    return result;
                }

                // Assign role
                var role = await _applicationDbContext.Roles.FirstOrDefaultAsync(x => x.Name == user.Role);

                if (role == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NonExistingRole);

                    return result;
                }

                var assignRoleToUser = await _userManager.AddToRoleAsync(identityUser, user.Role);

                if (!assignRoleToUser.Succeeded)
                {
                    await transaction.RollbackAsync();

                    result.ResponseCode = EDataResponseCode.GenericError;
                    result.ErrorMessage = string.Format(ResponseMessages.FailedToAssignRoleToUser, user.Role, identityUser.UserName, identityUser.Id);

                    return result;
                }

                await _applicationDbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;
                result.Data = identityUser.Id;

                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                result.Data = null;
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.UnsuccessfulCreationOfEntity, "User");

                return result;
            }
        }

        #endregion

        #region UPDATE

        /// <summary>
        /// Update a User.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> Update(UserDto userDto)
        {
            var result = new DataResponse<bool> { Data = false, Succeeded = false };

            await using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();

            try
            {
                var user = await _userManager.FindByIdAsync(userDto.Id);

                if (user == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NoDataFoundForKey, "User", userDto.Id);

                    return result;
                }

                // Validate and update email if provided
                if (!string.IsNullOrWhiteSpace(userDto.Email))
                {
                    var userEmailNotUnique = await _userManager.Users.AnyAsync(x =>
                        x.NormalizedEmail == userDto.Email.ToUpperInvariant() && x.Id != userDto.Id);

                    if (userEmailNotUnique)
                    {
                        result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                        result.ErrorMessage = string.Format(ResponseMessages.UserNotUpdatedEmailAlreadyExist, user.UserName, userDto.Email);

                        return result;
                    }

                    var userEmailUsedAsUsername = await _userManager.Users.AnyAsync(x =>
                        x.NormalizedUserName == userDto.Email.ToUpperInvariant() && x.Id != userDto.Id);

                    if (userEmailUsedAsUsername)
                    {
                        result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                        result.ErrorMessage = string.Format(ResponseMessages.UserNotUpdatedEmailAlreadyUsedAsUsernameFromOtherUser, user.UserName, userDto.Email);

                        return result;
                    }

                    user.Email = userDto.Email;
                    user.NormalizedEmail = user.Email.ToUpperInvariant();
                }

                // Update role if provided
                if (!string.IsNullOrEmpty(userDto.Role))
                {
                    if (userDto.Role != UserRoles.Admin && userDto.Role != UserRoles.User)
                    {
                        result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                        result.ErrorMessage = ResponseMessages.RoleMustBeAdminOrUser;

                        return result;
                    }

                    var existingRoles = await _userManager.GetRolesAsync(user);

                    if (existingRoles.Any())
                    {
                        await _userManager.RemoveFromRolesAsync(user, existingRoles);
                    }

                    var addRoleResult = await _userManager.AddToRoleAsync(user, userDto.Role);

                    if (!addRoleResult.Succeeded)
                    {
                        result.ResponseCode = EDataResponseCode.GenericError;
                        result.ErrorMessage = string.Format(ResponseMessages.UnsuccessfulUpdateOfEntity, "User");

                        return result;
                    }
                }

                if (!string.IsNullOrEmpty(userDto.PhoneNumber))
                {
                    user.PhoneNumber = userDto.PhoneNumber;
                }

                _applicationDbContext.Users.Update(user);

                await _applicationDbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;
                result.Data = true;

                return result;

            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.UnsuccessfulUpdateOfEntity, "User");

                return result;
            }
        }

        #endregion

        #region DELETE

        /// <summary>
        /// Delete User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> Delete(string Id)
        {
            var result = new DataResponse<bool> { Data = false, Succeeded = false };

            await using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();

            try
            {
                var user = await _userManager.FindByIdAsync(Id);

                if (user == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NoDataFoundForKey, "User", Id);

                    return result;
                }

                var deleteUser = await _userManager.DeleteAsync(user);

                if (!deleteUser.Succeeded)
                {
                    await transaction.RollbackAsync();
                    result.ResponseCode = EDataResponseCode.GenericError;
                    result.ErrorMessage = ResponseMessages.DeletionFailed;

                    return result;
                }

                await transaction.CommitAsync();

                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;
                result.Data = true;

                return result;

            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.DeletionFailed, "User");

                return result;
            }
        }

        #endregion
    }
}
