using Edge.Dtos;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Edge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        #region Declarations

        private readonly IOrdersService _ordersService;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="ordersService"></param>
        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        #endregion

        #region GET

        /// <summary>
        /// Get an Order by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var order = await _ordersService.Get(id);
                return Ok(Conversion<OrderDto>.ReturnResponse(order));
            }
            catch (Exception ex)
            {
                var errRet = new DataResponse<OrderDto>
                {
                    Data = null,
                    ResponseCode = EDataResponseCode.GenericError,
                    Succeeded = false,
                    ErrorMessage = ex.Message
                };
                return BadRequest(Conversion<OrderDto>.ReturnResponse(errRet));
            }
        }

        /// <summary>
        /// Get all Artworks by User Id..
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}/all")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            try
            {
                var orders = await _ordersService.GetAllByUserId(userId);
                return Ok(Conversion<List<OrderDto>>.ReturnResponse(orders));
            }
            catch (Exception ex)
            {
                var errRet = new DataResponse<List<OrderDto>>
                {
                    Data = null,
                    ResponseCode = EDataResponseCode.GenericError,
                    Succeeded = false,
                    ErrorMessage = ex.Message
                };
                return BadRequest(Conversion<List<OrderDto>>.ReturnResponse(errRet));
            }
        }

        #endregion
    }
}
