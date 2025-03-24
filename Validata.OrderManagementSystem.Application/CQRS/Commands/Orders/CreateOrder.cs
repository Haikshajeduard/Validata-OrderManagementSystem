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
    public partial class CreateOrderCommand : IRequest<bool>
    {
        public int CustomerId { get; set; }
        public List<AddItemModel> Items { get; set; }
    }
    public partial class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
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
        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request is null)
                    throw new ArgumentNullException("Request cannot be null");
                
                if(request.Items is null || request.Items.Count == 0)
                    throw new ArgumentNullException("Please provide at least one item");
                
                var selectedProductsAndQuantity = request.Items.ToDictionary(item => item.ProductId, item => item.Quantity);
                var selectedProducts = await _unitOfWork.Products.FindAsync(x=>selectedProductsAndQuantity.ContainsKey(x.Id));
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
                    TotalPrice = totalPrice
                };
                
                var customer = await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new ApplicationException(e.Message);
            }
        }
    }
}