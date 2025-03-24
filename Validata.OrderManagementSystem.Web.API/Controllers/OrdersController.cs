using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Validata.OrderManagementSystem.Application.CQRS.Commands.OrderItems;
using Validata.OrderManagementSystem.Application.CQRS.Commands.Orders;
using Validata.OrderManagementSystem.Application.CQRS.Queries.Orders;

namespace Validata.OrderManagementSystem.Web.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : BaseAPIController
    {
        [HttpGet]
        public virtual async Task<IActionResult> GetAll([FromQuery] GetOrders.GetOrdersQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }
        [HttpPost]
        public virtual async Task<IActionResult> CreateOrder([FromBody] CreateOrder.CreateOrderCommand model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerId = await Mediator.Send(model);

            //TODO: fix response models and status codes
            return Ok(customerId);
        }

        [HttpPost("add-item")]
        public virtual async Task<IActionResult> AddOrderItem([FromBody] AddOrderItem.AddOrderItemCommand model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerId = await Mediator.Send(model);

            //TODO: fix response models and status codes
            return Ok(customerId);
        }

        [HttpDelete]
        public virtual async Task<IActionResult> DeleteOrder([FromBody] DeleteOrder.DeleteOrderCommand model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerId = await Mediator.Send(model);

            //TODO: fix response models and status codes
            return Ok(customerId);
        }
    }
}
