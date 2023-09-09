using AutoMapper;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Domain.Customers;
using Wax.Core.Domain.Customers.Exceptions;
using Wax.Core.Repositories;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CreateCustomerResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryProvider _provider;

        public CreateCustomerCommandHandler(IMapper mapper, IRepositoryProvider provider)
        {
            _mapper = mapper;
            _provider = provider;
        }

        public async Task<CreateCustomerResponse> Handle(IReceiveContext<CreateCustomerCommand> context,
            CancellationToken cancellationToken)
        {
            var existing = await _provider.Customers.FindByNameAsync(context.Message.Name);

            if (existing != null)
            {
                throw new CustomerNameAlreadyExistsException();
            }

            var customer = _mapper.Map<Customer>(context.Message);

            await _provider.Customers.InsertAsync(customer, cancellationToken);

            return new CreateCustomerResponse { CustomerId = customer.Id };
        }
    }
}