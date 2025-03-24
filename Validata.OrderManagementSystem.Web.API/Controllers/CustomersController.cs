using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Validata.OrderManagementSystem.Application.CQRS.Commands.Customers;
using Validata.OrderManagementSystem.Application.CQRS.Queries.Customers;

namespace Validata.OrderManagementSystem.Web.API.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : BaseAPIController
    {
        [HttpGet]
        public virtual async Task<IActionResult> GetAll([FromQuery] GetCustomers.GetCustomersQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }
        [HttpPost]
        public virtual async Task<IActionResult> CreateCustomer([FromBody] CreateCustomer.CreateCustomerCommand model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerId = await Mediator.Send(model);

            //TODO: fix response models and status codes
            return Ok(customerId);
        }

        [HttpPut]
        public virtual async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomer.UpdateCustomerCommand model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerId = await Mediator.Send(model);

            //TODO: fix response models and status codes
            return Ok(customerId);
        }

        [HttpDelete]
        public virtual async Task<IActionResult> DeleteCustomer([FromBody] DeleteCustomer.DeleteCustomerCommand model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var deleted = await Mediator.Send(model);

            //TODO: fix response models and status codes
            return Ok(deleted);
        }
    }
}
