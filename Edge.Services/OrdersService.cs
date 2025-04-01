using Edge.Dtos;
using Edge.Repositories.Interfaces;
using Edge.Services.Interfaces;
using Edge.Shared.DataContracts.Responses;

namespace Edge.Services
{
    public class OrdersService : IOrdersService
    {
        #region Declarations

        private readonly IOrdersRepository _ordersRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="ordersRepository"></param>
        public OrdersService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Order by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Task<DataResponse<OrderDto>> Get(Guid Id) => _ordersRepository.Get(Id);

        /// <summary>
        /// Get all Orders by User Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<DataResponse<List<OrderDto>>> GetAllByUserId(string userId) => _ordersRepository.GetAllByUserId(userId);

        /// <summary>
        /// Create Order.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Task<DataResponse<Guid>> Create(OrderDto orderDto) => _ordersRepository.Create(orderDto);

        #endregion
    }
}
