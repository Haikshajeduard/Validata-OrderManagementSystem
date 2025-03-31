using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Validata.OrderManagementSystem.Tests.Integration.Controllers;

[TestFixture]
[Category("Integration")]
public class CustomersControllerTests : IntegrationTestBase
{
    [Test]
    public async Task CreateCustomer_ShouldReturnCustomerId()
    {
        var client = CreateClient();
        var customer = new { Name = "John Doe", Address = "123 Main St", PostalCode = 12345 };
        var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/customers", content);

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        Assert.IsNotEmpty(result);
    }
}
