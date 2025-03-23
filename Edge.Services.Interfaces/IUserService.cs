using Edge.Dtos;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Services.Interfaces
{
    public interface IUsersService
    {
        /// <summary>
        /// Get User by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DataResponse<UserDto>> Get(string id);

        /// <summary>
        /// Get all Users.
        /// </summary>
        /// <returns></returns>
        Task<DataResponse<List<UserDto>>> GetAll();

        /// <summary>
        /// Create a new User.
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <returns></returns>
        Task<DataResponse<string>> Create(CreateUserDto createUserDto);

        /// <summary>
        /// Update an existing User.
        /// </summary>
        /// <param name="updateUserDto"></param>
        /// <returns></returns>
        Task<DataResponse<bool>> Update(UserDto updateUserDto);

        /// <summary>
        /// Delete a User by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DataResponse<bool>> Delete(string id);
    }
}