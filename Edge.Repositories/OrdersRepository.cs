using AutoMapper;
using Edge.Data.EF;
using Edge.DomainModels;
using Edge.Dtos;
using Edge.Repositories.Interfaces;
using Edge.Shared.DataContracts.Enums;
using Edge.Shared.DataContracts.Resources;
using Edge.Shared.DataContracts.Responses;
using Microsoft.EntityFrameworkCore;

namespace Edge.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        #region Declarations

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="applicationDbContext"></param>
        /// <param name="mapper"></param>
        public OrdersRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        #endregion

        #region GET

        /// <summary>
        /// Get Order by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<DataResponse<OrderDto>> Get(Guid Id)
        {
            var result = new DataResponse<OrderDto>() { Data = null, Succeeded = false };

            try
            {
                var order = await _applicationDbContext.Orders.FirstOrDefaultAsync(x => x.Id == Id);

                if (order == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NoDataFoundForKey, nameof(Order), Id);
                    return result;
                }

                var orderDto = _mapper.Map<Order, OrderDto>(order);
                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;
                result.Data = orderDto;

                return result;
            }
            catch (Exception)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.GetEntityFailed, nameof(Order), Id);
                return result;
            }
        }

        /// <summary>
        /// Get all Orders by User Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<DataResponse<List<OrderDto>>> GetAllByUserId(string userId)
        {
            var result = new DataResponse<List<OrderDto>>() { Data = new List<OrderDto>(), Succeeded = false };

            try
            {
                var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NoDataFoundForKey, nameof(ApplicationUser), userId);
                    return result;
                }

                var orders = await _applicationDbContext.Orders.Where(x => x.UserId == userId).ToListAsync();

                if (orders == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NoDataFoundForKey, nameof(Order), userId);
                    return result;
                }

                var orderDtos = _mapper.Map<List<Order>, List<OrderDto>>(orders);

                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;
                result.Data = orderDtos;

                return result;
            }
            catch (Exception)
            {
                result.Data = null;
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.GettingEntitiesFailed, nameof(Order));

                return result;
            }
        }

        #endregion

        #region CREATE

        /// <summary>
        /// Create Order.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Task<DataResponse<OrderDto>> Create(OrderDto order)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region UPDATE

        /// <summary>
        /// Update Order.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Task<DataResponse<OrderDto>> Update(OrderDto order)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
