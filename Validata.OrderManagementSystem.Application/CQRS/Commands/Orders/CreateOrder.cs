using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Validata.OrderManagementSystem.Application.Models.Items;
using Validata.OrderManagementSystem.Domain.Entities;
using Validata.OrderManagementSystem.Persistence;
using Validata.OrderManagementSystem.Persistence.Repositories;

namespace Validata.OrderManagementSystem.Application.CQRS.Commands.Orders;

public class CreateOrder
{
    public partial class CreateOrderCommand : IRequest<int>
    {
        public int CustomerId { get; set; }
        public List<AddItemModel> Items { get; set; }
    }
    public partial class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly ILogger<CreateOrderCommandHandler> _logger;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApplicationDBContext _context;

        public CreateOrderCommandHandler(ILogger<CreateOrderCommandHandler> logger, UnitOfWork unitOfWork, IMapper mapper, IApplicationDBContext context)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }
        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request is null)
                    throw new ArgumentNullException("Request cannot be null");
                
                if(request.Items is null || request.Items.Count == 0)
                    throw new ArgumentNullException("Please provide at least one item");

                var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);
                if(customer is null)
                    throw new ArgumentNullException("Please provide customer");

                var selectedProductsAndQuantity = request.Items.ToDictionary(item => item.ProductId, item => item.Quantity);
                var selectedProductIds = selectedProductsAndQuantity.Keys.ToList();
                var selectedProducts = await _unitOfWork.Products.FindAsync(x=> selectedProductIds.Contains(x.Id));

                if(selectedProducts is null || selectedProducts.Count() == 0)
                    throw new ArgumentNullException("Please provide valid products");

                var totalPrice = selectedProducts.Sum(product => product.Price * selectedProductsAndQuantity[product.Id]);
                var order = new Order
                {
                    CustomerId = request.CustomerId,
                    OrderItems = request.Items.Select(x=> new OrderItem
                    {
                        Item = new Item{
                            ProductId = x.ProductId,
                            Quantity = x.Quantity
                        }
                    }).ToList(),
                    TotalPrice = totalPrice,
                    OrderDate = DateTime.UtcNow
                };
                
                order = await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();
                return order.Id;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new ApplicationException(e.Message);
            }
        }
    }
}