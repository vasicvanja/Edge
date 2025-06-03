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
                var order = await _applicationDbContext.Orders
                    .Include(x => x.OrderItems)
                    .ThenInclude(oi => oi.Artwork)
                    .FirstOrDefaultAsync(x => x.Id == Id);

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

                var orders = await _applicationDbContext.Orders
                    .Where(x => x.UserId == userId)
                    .Include(x => x.OrderItems)
                    .ThenInclude(oi => oi.Artwork)
                    .ToListAsync();

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
        public async Task<DataResponse<Guid>> Create(OrderDto orderDto)
        {
            var result = new DataResponse<Guid>();

            try
            {
                if (orderDto == null)
                {
                    result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                    result.ErrorMessage = string.Format(ResponseMessages.InvalidInputParameter, nameof(Order));
                    return result;
                }

                var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == orderDto.UserId);

                if (user == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = string.Format(ResponseMessages.NoDataFoundForKey, nameof(ApplicationUser), orderDto.UserId);
                    return result;
                }

                // Check if order with this PaymentIntentId already exists
                var existingOrder = await _applicationDbContext.Orders.FirstOrDefaultAsync(x => x.PaymentIntentId == orderDto.PaymentIntentId);

                if (existingOrder != null)
                {
                    result.ResponseCode = EDataResponseCode.InvalidInputParameter;
                    result.ErrorMessage = string.Format(ResponseMessages.EntityAlreadyExists, nameof(Order), orderDto.PaymentIntentId);
                    return result;
                }

                var order = new Order
                {
                    UserId = orderDto.UserId,
                    Amount = orderDto.Amount,
                    Status = orderDto.Status,
                    PaymentIntentId = orderDto.PaymentIntentId,
                    ReceiptUrl = orderDto.ReceiptUrl,
                    Description = orderDto.Description,
                    BillingAddress = orderDto.BillingAddress,
                    CreatedAt = DateTime.UtcNow,
                    Metadata = orderDto.Metadata,
                    OrderItems = new List<OrderItem>()
                };

                // Add order items for each artwork in the purchase
                if (orderDto.Metadata != null && orderDto.Metadata.TryGetValue("ArtworkIds", out var artworkIds))
                {
                    var ids = artworkIds.Split(',').Select(int.Parse).ToList();
                    var artworks = await _applicationDbContext.Artworks
                        .Where(a => ids.Contains(a.Id))
                        .ToListAsync();

                    foreach (var artwork in artworks)
                    {
                        order.OrderItems.Add(new OrderItem
                        {
                            ArtworkId = artwork.Id,
                            Price = artwork.Price,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }

                await _applicationDbContext.Orders.AddAsync(order);
                await _applicationDbContext.SaveChangesAsync();

                result.Data = order.Id;
                result.ResponseCode = EDataResponseCode.Success;
                result.Succeeded = true;

                return result;
            }
            catch (Exception)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = string.Format(ResponseMessages.UnsuccessfulCreationOfEntity, nameof(Order));
                return result;
            }
        }

        #endregion
    }
}
