using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using Validata.OrderManagementSystem.Application.CQRS.Commands.Customers;
using Validata.OrderManagementSystem.Domain.Entities;
using Validata.OrderManagementSystem.Persistence;
using Validata.OrderManagementSystem.Persistence.Repositories;
using Validata.OrderManagementSystem.Persistence.Repositories.OrderItems;

namespace Validata.OrderManagementSystem.Tests.Unit.Commands;

[TestFixture]
[Category("Unit")]
public class DeleteCustomerCommandHandlerTests : BaseTest
{
    private Mock<UnitOfWork> _unitOfWorkMock;
    private Mock<ILogger<DeleteCustomer.DeleteCustomerCommandHandler>> _loggerMock;
    private DeleteCustomer.DeleteCustomerCommandHandler _handler;

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

        _loggerMock = MockRepository.Create<ILogger<DeleteCustomer.DeleteCustomerCommandHandler>>();
        _handler = new DeleteCustomer.DeleteCustomerCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_ShouldDeleteCustomer_WhenCustomerExists()
    {
        var command = new DeleteCustomer.DeleteCustomerCommand { Id = 1 };
        var customer = new Customer { Id = 1, Name = "John Doe" };

        _unitOfWorkMock.Setup(x => x.Customers.GetByIdAsync(command.Id)).ReturnsAsync(customer);
        _unitOfWorkMock.Setup(x => x.Customers.Delete(customer));
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().Be(MediatR.Unit.Value);
        _unitOfWorkMock.Verify(x => x.Customers.GetByIdAsync(command.Id), Times.Once);
        _unitOfWorkMock.Verify(x => x.Customers.Delete(customer), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Handle_ShouldThrowException_WhenCustomerDoesNotExist()
    {
        var command = new DeleteCustomer.DeleteCustomerCommand { Id = 1 };

        _unitOfWorkMock.Setup(x => x.Customers.GetByIdAsync(command.Id)).ReturnsAsync((Customer)null);

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        act.Should().ThrowAsync<Exception>().Result.WithMessage("Customer not found");
    }

    [Test]
    public async Task Handle_ShouldLogInformation_WhenCustomerIsDeleted()
    {
        var command = new DeleteCustomer.DeleteCustomerCommand { Id = 1 };
        var customer = new Customer { Id = 1, Name = "John Doe" };

        _unitOfWorkMock.Setup(x => x.Customers.GetByIdAsync(command.Id)).ReturnsAsync(customer);
        _unitOfWorkMock.Setup(x => x.Customers.Delete(customer));
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().Be(MediatR.Unit.Value);
        _loggerMock.Verify(x => x.LogInformation($"Customer {customer.Name} with id: {customer.Id} deleted"), Times.Once);
    }
}
