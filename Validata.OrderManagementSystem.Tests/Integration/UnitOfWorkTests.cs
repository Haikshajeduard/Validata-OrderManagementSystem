using NUnit.Framework;
using System.Threading.Tasks;
using Validata.OrderManagementSystem.Persistence;

namespace Validata.OrderManagementSystem.Tests.Integration;

[TestFixture]
[Category("Integration")]
public class UnitOfWorkTests : IntegrationTestBase
{
    [Test]
    public async Task SaveChangesAsync_ShouldPersistChanges()
    {
        // Arrange
        using var scope = CreateScope();
        var unitOfWork = scope.ServiceProvider.GetService<UnitOfWork>();
        var customer = new Domain.Entities.Customer { Name = "Test Customer", Address = "Test Address", PostalCode = 12345 };

        // Act
        await unitOfWork.Customers.AddAsync(customer);
        var result = await unitOfWork.SaveChangesAsync();

        // Assert
        Assert.AreEqual(1, result);
    }
}
