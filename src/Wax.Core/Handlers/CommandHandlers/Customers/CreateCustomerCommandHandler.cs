using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Services.Customers;
using Wax.Messages.Commands.Customers;
using Wax.Messages.Events.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand>
    {
        private readonly ICustomerService _customerService;

        public CreateCustomerCommandHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task Handle(IReceiveContext<CreateCustomerCommand> context, CancellationToken cancellationToken)
        {
            var @event = await _customerService.CreateAsync(context.Message);

            await context.PublishAsync(@event, cancellationToken);
        }
    }
}