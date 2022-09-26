using Mediator.Net;
using Microsoft.AspNetCore.Mvc;
using Wax.Messages.Commands.Customers;
using Wax.Messages.Dtos.Customers;
using Wax.Messages.Requests.Customers;

namespace Wax.Api.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CustomerShortInfo), 200)]
        public async Task<IActionResult> GetListAsync(Guid id)
        {
            var response = await _mediator.RequestAsync<GetCustomerRequest, GetCustomerResponse>(
                new GetCustomerRequest { CustomerId = id });

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCustomerCommand command)
        {
            await _mediator.SendAsync(command);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCustomerCommand command)
        {
            await _mediator.SendAsync(command);
            return Ok();
        }
    }
}