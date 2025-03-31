using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using Validata.OrderManagementSystem.Application.CQRS.Commands.Orders;
using Validata.OrderManagementSystem.Domain.Entities;
using Validata.OrderManagementSystem.Persistence;
using Validata.OrderManagementSystem.Persistence.Repositories;
using Validata.OrderManagementSystem.Persistence.Repositories.OrderItems;

namespace Validata.OrderManagementSystem.Tests.Unit.Commands;

[TestFixture]
[Category("Unit")]
public class DeleteOrderCommandHandlerTests : BaseTest
{
    private Mock<UnitOfWork> _unitOfWorkMock;
    private Mock<ILogger<DeleteOrder.DeleteOrderCommandHandler>> _loggerMock;
    private DeleteOrder.DeleteOrderCommandHandler _handler;

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

        _loggerMock = MockRepository.Create<ILogger<DeleteOrder.DeleteOrderCommandHandler>>();
        _handler = new DeleteOrder.DeleteOrderCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_ShouldDeleteOrder_WhenOrderExists()
    {
        var command = new DeleteOrder.DeleteOrderCommand { Id = 1 };
        var order = new Order { Id = 1 };

        _unitOfWorkMock.Setup(x => x.Orders.GetByIdAsync(command.Id)).ReturnsAsync(order);
        _unitOfWorkMock.Setup(x => x.Orders.Delete(order));
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().Be(MediatR.Unit.Value);
        _unitOfWorkMock.Verify(x => x.Orders.GetByIdAsync(command.Id), Times.Once);
        _unitOfWorkMock.Verify(x => x.Orders.Delete(order), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Handle_ShouldThrowException_WhenOrderDoesNotExist()
    {
        var command = new DeleteOrder.DeleteOrderCommand { Id = 1 };

        _unitOfWorkMock.Setup(x => x.Orders.GetByIdAsync(command.Id)).ReturnsAsync((Order)null);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        act.Should().ThrowAsync<Exception>().Result.WithMessage("Order not found");
    }
}
