using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Validata.OrderManagementSystem.Application.Models.Orders;
using Validata.OrderManagementSystem.Persistence;

namespace Validata.OrderManagementSystem.Application.CQRS.Queries.Orders;

public class GetOrders
{
    public partial class GetOrdersQuery : IRequest<IEnumerable<OrderModel>>
    {
        public int CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
    }

    public partial class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderModel>>
    {
        private readonly IApplicationDBContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOrdersQueryHandler> _logger;

        public GetOrdersQueryHandler(IApplicationDBContext applicationDbContext, IMapper mapper, ILogger<GetOrdersQueryHandler> logger)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<OrderModel>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var ordersQuery = _applicationDbContext.Orders
                    .Include(x => x.Customer)
                    .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Item)
                    .ThenInclude(x => x.Product)
                    .Where(x => x.CustomerId == request.CustomerId);

                if (request.OrderDate.HasValue)
                    ordersQuery = ordersQuery.Where(x => x.OrderDate.Date == request.OrderDate.Value.Date);

                var orders = await ordersQuery.OrderByDescending(x => x.OrderDate).ToListAsync();

                var ordersModel = _mapper.Map<List<OrderModel>>(orders);
                return ordersModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApplicationException(ex.Message);
            }
        }
    }
}