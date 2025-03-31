using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Validata.OrderManagementSystem.Tests.Integration.Controllers;

[TestFixture]
[Category("Integration")]
public class OrdersControllerTests : IntegrationTestBase
{
    [Test]
    public async Task CreateOrder_ShouldReturnOrderId()
    {
        var client = CreateClient();
        var order = new
        {
            CustomerId = 1,
            Items = new[] { new { ProductId = 1, Quantity = 2 } }
        };
        var content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/orders", content);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        Assert.IsNotEmpty(result);
    }
}
