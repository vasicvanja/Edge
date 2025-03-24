using Edge.Shared.DataContracts.Responses;
using Microsoft.AspNetCore.Identity;

namespace Edge.Repositories.Interfaces
{
    public interface IRolesRepository
    {
        /// <summary>
        /// Get all Roles.
        /// </summary>
        /// <returns></returns>
        Task<DataResponse<List<IdentityRole>>> GetAll();
    }
}
