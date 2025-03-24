using Edge.Shared.DataContracts.Responses;
using Microsoft.AspNetCore.Identity;

namespace Edge.Services.Interfaces
{
    public interface IRolesService
    {
        /// <summary>
        /// Get all Roles.
        /// </summary>
        /// <returns></returns>
        Task<DataResponse<List<IdentityRole>>> GetAll();
    }
}
