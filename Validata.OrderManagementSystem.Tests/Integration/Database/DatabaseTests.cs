using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Validata.OrderManagementSystem.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Validata.OrderManagementSystem.Tests.Integration.Database;

[TestFixture]
[Category("Integration")]
public class DatabaseTests : IntegrationTestBase
{
    [Test]
    public async Task SeedProducts_ShouldAddProductsToDatabase()
    {
        using var scope = CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();

        var products = await dbContext.Products.ToListAsync();

        Assert.IsNotEmpty(products);
    }

    [Test]
    public async Task AddCustomer_ShouldPersistToDatabase()
    {
        using var scope = CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ApplicationDBContext>();
        var customer = new Domain.Entities.Customer { Name = "Jane Doe", Address = "456 Elm St", PostalCode = 67890 };

        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();

        var savedCustomer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Name == "Jane Doe");
        Assert.IsNotNull(savedCustomer);
    }
}
