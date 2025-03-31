using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace Validata.OrderManagementSystem.Tests.Integration.Controllers;

[TestFixture]
[Category("Integration")]
public class ProductsControllerTests : IntegrationTestBase
{
    [Test]
    public async Task GetAll_ShouldReturnProducts()
    {
        // Arrange
        var client = CreateClient();

        // Act
        var response = await client.GetAsync("/api/products");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.IsNotEmpty(content);
    }
}
