using Edge.Dtos;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Services.Interfaces
{
    public interface IOrdersService
    {
        /// <summary>
        /// Get Order by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<DataResponse<OrderDto>> Get(Guid Id);

        /// <summary>
        /// Get all Orders by User Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<DataResponse<List<OrderDto>>> GetAllByUserId(string userId);

        /// <summary>
        /// Create Order.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<DataResponse<Guid>> Create(OrderDto orderDto);
    }
}
