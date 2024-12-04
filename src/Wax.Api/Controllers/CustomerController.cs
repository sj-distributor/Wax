using Mediator.Net;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponse<CustomerShortInfo>), 200)]
        public async Task<IActionResult> GetListAsync([FromQuery] GetCustomersRequest request)
        {
            var response =
                await _mediator.RequestAsync<GetCustomersRequest, PaginatedResponse<CustomerShortInfo>>(request);

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateCustomerResponse), 200)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCustomerCommand command)
        {
            var response = await _mediator.SendAsync<CreateCustomerCommand, CreateCustomerResponse>(command);
            return CreatedAtRoute(nameof(GetListAsync), response);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCustomerCommand command)
        {
            await _mediator.SendAsync(command);
            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteCustomerCommand command)
        {
            await _mediator.SendAsync(command);
            return NoContent();
        }
    }
}