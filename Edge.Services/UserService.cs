using Edge.Dtos;
using Edge.Repositories.Interfaces;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Services
{
    public class UsersService : IUsersService
    {
        #region Declarations

        private readonly IUsersRepository _usersRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="userRepository"></param>
        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get User by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DataResponse<UserDto>> Get(string id) => await _usersRepository.Get(id);

        /// <summary>
        /// Get all Users.
        /// </summary>
        /// <returns></returns>
        public async Task<DataResponse<List<UserDto>>> GetAll() => await _usersRepository.GetAll();

        /// <summary>
        /// Create new User.
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <returns></returns>
        public async Task<DataResponse<string>> Create(CreateUserDto createUserDto) => await _usersRepository.Create(createUserDto);

        /// <summary>
        /// Updates existing User.
        /// </summary>
        /// <param name="updateUserDto"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> Update(UserDto updateUserDto) => await _usersRepository.Update(updateUserDto);

        /// <summary>
        /// Enable or disable a User.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public async Task<DataResponse<bool>> EnableDisableUser(string id, bool enabled) => await _usersRepository.EnableDisableUser(id, enabled);

        /// <summary>
        /// Delete a User.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<DataResponse<bool>> Delete(string id) => _usersRepository.Delete(id);

        #endregion
    }
}
