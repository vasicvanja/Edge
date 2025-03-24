using Edge.Repositories.Interfaces;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Responses;
using Microsoft.AspNetCore.Identity;

namespace Edge.Services
{
    public class RolesService : IRolesService
    {
        #region Declarations

        private readonly IRolesRepository _rolesRepository;

        #endregion

        #region Ctor.

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="rolesRepository"></param>
        public RolesService(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        #endregion

        #region Methods

        public async Task<DataResponse<List<IdentityRole>>> GetAll() => await _rolesRepository.GetAll();

        #endregion
    }
}
