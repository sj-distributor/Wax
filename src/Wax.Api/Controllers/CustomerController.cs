using Mediator.Net;
using Microsoft.AspNetCore.Mvc;
using Wax.Messages;
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
        [ProducesResponseType(typeof(UniformResponse<CustomerShortInfo>), 200)]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var response = await _mediator.RequestAsync<GetCustomerRequest, UniformResponse<CustomerShortInfo>>(
                new GetCustomerRequest {CustomerId = id});

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UniformResponse<Guid>), 200)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCustomerCommand command)
        {
            var response = await _mediator.SendAsync<CreateCustomerCommand, UniformResponse<Guid>>(command);
            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(UniformResponse), 200)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCustomerCommand command)
        {
            var response = await _mediator.SendAsync<UpdateCustomerCommand, UniformResponse>(command);
            return Ok(response);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(UniformResponse), 200)]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteCustomerCommand command)
        {
            var response = await _mediator.SendAsync<DeleteCustomerCommand, UniformResponse>(command);
            return Ok(response);
        }
    }
}