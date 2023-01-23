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
        [ProducesResponseType(typeof(IUniformResponse<CustomerShortInfo>), 200)]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var response = await _mediator.RequestAsync<GetCustomerRequest, IUniformResponse<CustomerShortInfo>>(
                new GetCustomerRequest {CustomerId = id});

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IUniformResponse<Guid>), 200)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCustomerCommand command)
        {
            var response = await _mediator.SendAsync<CreateCustomerCommand, IUniformResponse<Guid>>(command);
            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(IUniformResponse), 200)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCustomerCommand command)
        {
            await _mediator.SendAsync<UpdateCustomerCommand, IUniformResponse>(command);
            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(typeof(IUniformResponse), 200)]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteCustomerCommand command)
        {
            await _mediator.SendAsync<DeleteCustomerCommand, IUniformResponse>(command);
            return Ok();
        }
    }
}