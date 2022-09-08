using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Services.Emails;
using Wax.Messages.Events.Customers;

namespace Wax.Core.Handlers.EventHandlers.Customers
{
    public class CustomerCreatedEventHandler : IEventHandler<CustomerCreatedEvent>
    {
        private readonly IEmailService _emailService;

        public CustomerCreatedEventHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task Handle(IReceiveContext<CustomerCreatedEvent> context, CancellationToken cancellationToken)
        {
            return _emailService.SendAsync("", "", "Customer created", $"{context.Message.Name} created successfully!");
        }
    }
}