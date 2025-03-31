using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using Validata.OrderManagementSystem.Application.CQRS.Commands.Customers;
using Validata.OrderManagementSystem.Domain.Entities;
using Validata.OrderManagementSystem.Persistence;

namespace Validata.OrderManagementSystem.Tests.Unit.Commands;

[TestFixture]
[Category("Unit")]
public class CreateCustomerCommandHandlerTests : BaseTest
{
    private Mock<UnitOfWork> _unitOfWorkMock;
    private Mock<ILogger<CreateCustomer.CreateCustomerCommandHandler>> _loggerMock;
    private CreateCustomer.CreateCustomerCommandHandler _handler;

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

        _loggerMock = MockRepository.Create<ILogger<CreateCustomer.CreateCustomerCommandHandler>>();
        _handler = new CreateCustomer.CreateCustomerCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCreateCustomer_WhenRequestIsValid()
    {
        // Arrange
        var command = new CreateCustomer.CreateCustomerCommand
        {
            Name = "John Doe",
            Address = "123 Main St",
            PostalCode = 12345
        };

        var customer = new Customer
        {
            Id = 1,
            Name = command.Name,
            Address = command.Address,
            PostalCode = command.PostalCode
        };

        _unitOfWorkMock.Setup(x => x.Customers.AddAsync(It.IsAny<Customer>())).ReturnsAsync(customer);
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(customer.Id);
        _unitOfWorkMock.Verify(x => x.Customers.AddAsync(It.Is<Customer>(c =>
            c.Name == command.Name &&
            c.Address == command.Address &&
            c.PostalCode == command.PostalCode)), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Handle_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Arrange
        CreateCustomer.CreateCustomerCommand command = null;

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Request cannot be null");
    }

    [Test]
    public async Task Handle_ShouldLogError_WhenExceptionOccurs()
    {
        // Arrange
        var command = new CreateCustomer.CreateCustomerCommand
        {
            Name = "John Doe",
            Address = "123 Main St",
            PostalCode = 12345
        };

        _unitOfWorkMock.Setup(x => x.Customers.AddAsync(It.IsAny<Customer>())).ThrowsAsync(new Exception("Database error"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ApplicationException>().WithMessage("Database error");
        _loggerMock.Verify(x => x.LogError(It.IsAny<Exception>(), "Database error"), Times.Once);
    }

    [Test]
    public async Task Handle_ShouldThrowApplicationException_WhenSaveChangesFails()
    {
        // Arrange
        var command = new CreateCustomer.CreateCustomerCommand
        {
            Name = "John Doe",
            Address = "123 Main St",
            PostalCode = 12345
        };

        var customer = new Customer
        {
            Id = 1,
            Name = command.Name,
            Address = command.Address,
            PostalCode = command.PostalCode
        };

        _unitOfWorkMock.Setup(x => x.Customers.AddAsync(It.IsAny<Customer>())).ReturnsAsync(customer);
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ApplicationException>().WithMessage("Failed to save changes.");
    }
}
