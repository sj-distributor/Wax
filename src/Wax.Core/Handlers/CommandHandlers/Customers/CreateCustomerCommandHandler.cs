using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Entities.Customers;
using Wax.Core.Services.Customers;
using Wax.Core.Services.Customers.Exceptions;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand>
    {
        private readonly ICustomerDataProvider _customerDataProvider;

        public CreateCustomerCommandHandler(ICustomerDataProvider customerDataProvider)
        {
            _customerDataProvider = customerDataProvider;
        }

        public async Task Handle(IReceiveContext<CreateCustomerCommand> context, CancellationToken cancellationToken)
        {
            var customer = await _customerDataProvider.GetByNameAsync(context.Message.Name);

            if (customer != null)
            {
                throw new CustomerNameAlreadyExistsException();
            }

            customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = context.Message.Name
            };

            await _customerDataProvider.AddAsync(customer);
        }
    }
}