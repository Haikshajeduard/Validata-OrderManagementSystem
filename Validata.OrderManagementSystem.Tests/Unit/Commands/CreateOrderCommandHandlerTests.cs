using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Validata.OrderManagementSystem.Application.CQRS.Commands.Orders;
using Validata.OrderManagementSystem.Application.Models.Items;
using Validata.OrderManagementSystem.Domain.Entities;
using Validata.OrderManagementSystem.Persistence;

namespace Validata.OrderManagementSystem.Tests.Unit.Commands;

[TestFixture]
[Category("Unit")]
public class CreateOrderCommandHandlerTests : BaseTest
{
    private Mock<UnitOfWork> _unitOfWorkMock;
    private Mock<ILogger<CreateOrder.CreateOrderCommandHandler>> _loggerMock;
    private CreateOrder.CreateOrderCommandHandler _handler;

    [SetUp]
    public void Init()
    {
        var dbContextMock = MockRepository.Create<IApplicationDBContext>();
        var productRepoMock = MockRepository.Create<IRepository<Product>>();
        var orderRepoMock = MockRepository.Create<IRepository<Order>>();
        var itemRepoMock = MockRepository.Create<IRepository<Item>>();
        var orderItemRepoMock = MockRepository.Create<IOrderItemRepository>();
        var customerRepoMock = MockRepository.Create<IRepository<Customer>>();

        _unitOfWorkMock = MockRepository.Create<UnitOfWork>(
            dbContextMock.Object,
            productRepoMock.Object,
            orderRepoMock.Object,
            itemRepoMock.Object,
            orderItemRepoMock.Object,
            customerRepoMock.Object
        );

        _loggerMock = MockRepository.Create<ILogger<CreateOrder.CreateOrderCommandHandler>>();
        _handler = new CreateOrder.CreateOrderCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, null, null);
    }

    [Test]
    public async Task Handle_ShouldCreateOrder_WhenRequestIsValid()
    {
        // Arrange
        var command = new CreateOrder.CreateOrderCommand
        {
            CustomerId = 1,
            Items = new List<AddItemModel>
            {
                new AddItemModel { ProductId = 1, Quantity = 2 }
            }
        };

        var customer = new Customer { Id = 1 };
        var product = new Product { Id = 1, Price = 100 };
        var order = new Order { Id = 1, TotalPrice = 200 };

        _unitOfWorkMock.Setup(x => x.Customers.GetByIdAsync(command.CustomerId)).ReturnsAsync(customer);
        _unitOfWorkMock.Setup(x => x.Products.FindAsync(It.IsAny<Func<Product, bool>>())).ReturnsAsync(new List<Product> { product });
        _unitOfWorkMock.Setup(x => x.Orders.AddAsync(It.IsAny<Order>())).ReturnsAsync(order);
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(order.Id);
        _unitOfWorkMock.Verify(x => x.Orders.AddAsync(It.Is<Order>(o => o.TotalPrice == 200)), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Handle_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
        CreateOrder.CreateOrderCommand command = null;

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Request cannot be null");
    }
}
