using Mediator.Net;
using Microsoft.AspNetCore.Mvc;
using Wax.Messages.Commands.Customers;
using Wax.Messages.Dtos.Customers;
using Wax.Messages.Requests.Customers;
using ILogger = Serilog.ILogger;

namespace Wax.Api.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public CustomerController(IMediator mediator,ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
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
            var response = await _mediator.SendAsync<CreateCustomerCommand, CreateCustomerResponse>(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateCustomerCommand command)
        {
            await _mediator.SendAsync(command);
            return Ok();
        }
    }
}