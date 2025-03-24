using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Constants;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Edge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        #region Declarations

        private readonly IRolesService _rolesService;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="rolesService"></param>
        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        #endregion

        #region GET

        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var roles = await _rolesService.GetAll();
                return Ok(Conversion<List<IdentityRole>>.ReturnResponse(roles));
            }
            catch (Exception ex)
            {
                var errRet = new DataResponse<List<IdentityRole>>
                {
                    Data = null,
                    ResponseCode = EDataResponseCode.GenericError,
                    Succeeded = false,
                    ErrorMessage = ex.Message
                };
                return BadRequest(Conversion<List<IdentityRole>>.ReturnResponse(errRet));
            }
        }

        #endregion
    }
}
