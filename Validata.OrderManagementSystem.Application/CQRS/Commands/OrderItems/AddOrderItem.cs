using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validata.OrderManagementSystem.Application.Models.Items;
using Validata.OrderManagementSystem.Domain.Entities;
using Validata.OrderManagementSystem.Persistence;

namespace Validata.OrderManagementSystem.Application.CQRS.Commands.OrderItems
{
    public class AddOrderItem
    {
        public partial class AddOrderItemCommand : IRequest<int>
        {
            public int CustomerId { get; set; }
            public int OrderId { get; set; }
            public AddItemModel Item { get; set; }
        }
        public partial class AddOrderItemCommandHandler : IRequestHandler<AddOrderItemCommand, int>
        {
            private readonly ILogger<AddOrderItemCommandHandler> _logger;
            private readonly UnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public AddOrderItemCommandHandler(ILogger<AddOrderItemCommandHandler> logger, UnitOfWork unitOfWork, IMapper mapper)
            {
                _logger = logger;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }
            public async Task<int> Handle(AddOrderItemCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    if (request is null)
                        throw new ArgumentNullException("Request cannot be null");

                    if (request.Item is null)
                        throw new ArgumentNullException("Please provide item");

                    var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);
                    if (customer is null)
                        throw new ArgumentNullException("Please provide customer");

                    var selectedProduct = await _unitOfWork.Products.GetByIdAsync(request.Item.ProductId);

                    if (selectedProduct is null)
                        throw new ArgumentNullException("Please provide valid product");

                    var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
                    if (order is null)
                        throw new ArgumentNullException("Please provide valid order number");

                    var orderItems = await _unitOfWork.OrderItems.GetOrderItems(request.OrderId);

                    if (!(orderItems?.Any() ?? false))
                        throw new ArgumentNullException("Order has no initial items");

                    var existingOrderItem = orderItems.FirstOrDefault(x => x.Item.ProductId == request.Item.ProductId);

                    if (existingOrderItem is null)
                    {
                        await _unitOfWork.OrderItems.AddAsync(new OrderItem
                        {
                            Order = order,
                            Item = new Item
                            {
                                ProductId = request.Item.ProductId,
                                Quantity = request.Item.Quantity
                            }
                        });
                    }
                    else
                    {
                        existingOrderItem.Item.Quantity += request.Item.Quantity;
                        _unitOfWork.OrderItems.Update(existingOrderItem);
                    }

                    var increasePriceValue = selectedProduct.Price * request.Item.Quantity;
                    order.TotalPrice += increasePriceValue;
                    _unitOfWork.Orders.Update(order);

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
}
