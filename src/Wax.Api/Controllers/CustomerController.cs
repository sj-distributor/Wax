using Mediator.Net;
using Microsoft.AspNetCore.Mvc;
using Wax.Messages.Commands.Customers;

namespace Wax.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCustomerCommand command)
        {
            await _mediator.SendAsync(command);
            return Ok();
        }
    }
}