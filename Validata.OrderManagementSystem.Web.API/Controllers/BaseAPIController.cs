using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Validata.OrderManagementSystem.Web.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BaseAPIController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
    }
}
