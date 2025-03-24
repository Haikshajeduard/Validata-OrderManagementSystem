using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Validata.OrderManagementSystem.Application.CQRS.Queries.Products;

namespace Validata.OrderManagementSystem.Web.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : BaseAPIController
    {
        [HttpGet]
        public virtual async Task<IActionResult> GetAll([FromQuery] GetProducts.GetProductsQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}
