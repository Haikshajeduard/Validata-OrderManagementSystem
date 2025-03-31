using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using Validata.OrderManagementSystem.Application.Models.Customers;
using Validata.OrderManagementSystem.Domain.Entities;

namespace Validata.OrderManagementSystem.Tests.Unit.Mappings;

[TestFixture]
[Category("Unit")]
public class CustomerMappingTests
{
    private IMapper _mapper;

    [SetUp]
    public void Init()
    {
        var config = new MapperConfiguration(cfg => cfg.CreateMap<Customer, CustomerModel>());
        _mapper = config.CreateMapper();
    }

    [Test]
    public void CustomerToCustomerModel_ShouldMapCorrectly()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            Name = "John Doe",
            Address = "123 Main St",
            PostalCode = 12345
        };

        // Act
        var result = _mapper.Map<CustomerModel>(customer);

        // Assert
        result.Name.Should().Be(customer.Name);
        result.Address.Should().Be(customer.Address);
        result.PostalCode.Should().Be(customer.PostalCode);
    }
}
