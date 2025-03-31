using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Validata.OrderManagementSystem.Application.CQRS.Queries.Customers;
using Validata.OrderManagementSystem.Application.Models.Customers;
using Validata.OrderManagementSystem.Domain.Entities;
using Validata.OrderManagementSystem.Persistence;
using Validata.OrderManagementSystem.Persistence.Repositories;
using Validata.OrderManagementSystem.Persistence.Repositories.OrderItems;

namespace Validata.OrderManagementSystem.Tests.Unit.Queries;

[TestFixture]
[Category("Unit")]
public class GetCustomersQueryHandlerTests : BaseTest
{
    private Mock<UnitOfWork> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ILogger<GetCustomers.GetCustomersQueryHandler>> _loggerMock;
    private GetCustomers.GetCustomersQueryHandler _handler;

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

        _mapperMock = MockRepository.Create<IMapper>();
        _loggerMock = MockRepository.Create<ILogger<GetCustomers.GetCustomersQueryHandler>>();
        _handler = new GetCustomers.GetCustomersQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnCustomers_WhenCustomersExist()
    {
        var customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "John Doe", Address = "123 Main St", PostalCode = 12345 },
            new Customer { Id = 2, Name = "Jane Smith", Address = "456 Elm St", PostalCode = 67890 }
        };

        var customerModels = new List<CustomerModel>
        {
            new CustomerModel { Name = "John Doe", Address = "123 Main St", PostalCode = 12345 },
            new CustomerModel { Name = "Jane Smith", Address = "456 Elm St", PostalCode = 67890 }
        };

        _unitOfWorkMock.Setup(x => x.Customers.GetAllAsync()).ReturnsAsync(customers);
        _mapperMock.Setup(x => x.Map<IEnumerable<CustomerModel>>(customers)).Returns(customerModels);

        var result = await _handler.Handle(new GetCustomers.GetCustomersQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(customerModels);
        _unitOfWorkMock.Verify(x => x.Customers.GetAllAsync(), Times.Once);
        _mapperMock.Verify(x => x.Map<IEnumerable<CustomerModel>>(customers), Times.Once);
    }

    [Test]
    public void Handle_ShouldLogError_WhenExceptionOccurs()
    {
        _unitOfWorkMock.Setup(x => x.Customers.GetAllAsync()).ThrowsAsync(new Exception("Database error"));

        Func<Task> act = async () => await _handler.Handle(new GetCustomers.GetCustomersQuery(), CancellationToken.None);

        act.Should().ThrowAsync<ApplicationException>().Result.WithMessage("Database error");
        _loggerMock.Verify(x => x.LogError(It.IsAny<Exception>(), "Database error"), Times.Once);
    }
}
