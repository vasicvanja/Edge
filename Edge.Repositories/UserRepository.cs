using CustomValidation.Interface;
using Edge.Data.EF;
using Edge.DomainModels;
using Edge.Dtos;
using Edge.Repositories.Interfaces;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Resources;
using Edge.Shared.DataContracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Edge.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        #region Declarations

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailValidator _emailValidator;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="applicationDbContext"></param>
        /// <param name="emailValidator"></param>
        /// <param name="httpContextAccessor"></param>
        public UsersRepository(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor, IEmailValidator emailValidator)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
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
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = roles[0],
                    Enabled = user.Enabled,
                    ConcurrencyStamp = user.ConcurrencyStamp,
                    DateCreated = user.DateCreated,
                    DateModified = user.DateModified,
                    CreatedBy = user.CreatedBy,
                    ModifiedBy = user.ModifiedBy
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
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Role = role[0],
                        Enabled = user.Enabled,
                        ConcurrencyStamp = user.ConcurrencyStamp,
                        DateCreated = user.DateCreated,
                        DateModified = user.DateModified,
                        CreatedBy = user.CreatedBy,
                        ModifiedBy = user.ModifiedBy
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
                    result.ErrorMessage = string.Format(ResponseMessages.UsernameAlreadyTaken, user.UserName);
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
                var identityUser = new ApplicationUser
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    NormalizedUserName = user.UserName.ToUpperInvariant(),
                    NormalizedEmail = user.Email.ToUpperInvariant(),
                    Enabled = user.Enabled,
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

                if ((string.IsNullOrEmpty(userDto.ConcurrencyStamp)) ||
                    !(userDto.ConcurrencyStamp.Equals(user.ConcurrencyStamp)))
                {
                    result.ResponseCode = EDataResponseCode.StaleObjectState;
                    result.ErrorMessage = string.Format(ResponseMessages
                        .The_record_you_are_working_on_has_been_modified_by_another_user_Changes_you_have_made_have_not_been_saved_please_resubmit,
                        nameof(ApplicationUser));
                    
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
                    var existingRoles = await _userManager.GetRolesAsync(user);

                    if (existingRoles.Any())
                    {
                        await _userManager.RemoveFromRolesAsync(user, existingRoles);
                    }

                    var addRoleResult = await _userManager.AddToRoleAsync(user, userDto.Role);

                    if (!addRoleResult.Succeeded)
                    {
                        result.ResponseCode = EDataResponseCode.GenericError;
                        result.ErrorMessage = string.Format(ResponseMessages.UnsuccessfulUpdateOfEntity, nameof(ApplicationUser));

                        return result;
                    }
                }

                if (!string.IsNullOrEmpty(userDto.PhoneNumber))
                {
                    user.PhoneNumber = userDto.PhoneNumber;
                }

                user.Enabled = userDto.Enabled;
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;

                var currentUser = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
                user.DateModified = DateTime.UtcNow;
                user.ModifiedBy = currentUser;
                user.DateCreated = userDto.DateCreated;
                user.CreatedBy = userDto.CreatedBy;

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
                result.ErrorMessage = string.Format(ResponseMessages.UnsuccessfulUpdateOfEntity, nameof(ApplicationUser));

                return result;
            }
        }

        /// <summary>
        /// Enable or disable a User.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> EnableDisableUser(string id, bool enabled)
        {
            var result = new DataResponse<bool> { Data = false, Succeeded = false };

            await using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();

            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NoDataFoundForKey, nameof(ApplicationUser), id);

                    return result;
                }

                user.Enabled = enabled;

                var currentUser = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
                user.DateModified = DateTime.UtcNow;
                user.ModifiedBy = currentUser;

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
                result.ErrorMessage = string.Format(ResponseMessages.UnsuccessfulUpdateOfEntity, nameof(ApplicationUser));

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
                result.ErrorMessage = string.Format(ResponseMessages.DeletionFailed, nameof(ApplicationUser), Id);

                return result;
            }
        }

        #endregion
    }
}
