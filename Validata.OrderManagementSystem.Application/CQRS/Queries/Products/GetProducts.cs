using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Validata.OrderManagementSystem.Application.Models.Orders;
using Validata.OrderManagementSystem.Application.Models.Products;
using Validata.OrderManagementSystem.Persistence;

namespace Validata.OrderManagementSystem.Application.CQRS.Queries.Products;

public class GetProducts
{
    public partial class GetProductsQuery : IRequest<IEnumerable<ProductModel>>
    {
    }

    public partial class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductModel>>
    {
        private readonly IApplicationDBContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetProductsQueryHandler> _logger;

        public GetProductsQueryHandler(IApplicationDBContext applicationDbContext, IMapper mapper, ILogger<GetProductsQueryHandler> logger)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<ProductModel>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var products = await _applicationDbContext.Products.ToListAsync();
                var productsModel = _mapper.Map<List<ProductModel>>(products);
                return productsModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApplicationException(ex.Message);
            }
        }
    }
}