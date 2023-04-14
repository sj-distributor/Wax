using AutoMapper;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Data;
using Wax.Core.Domain.Customers;
using Wax.Core.Domain.Customers.Exceptions;
using Wax.Core.Repositories;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CreateCustomerResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHandler(IMapper mapper,ICustomerRepository customerRepository)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
        }

        public async Task<CreateCustomerResponse> Handle(IReceiveContext<CreateCustomerCommand> context,
            CancellationToken cancellationToken)
        {
            var existing = await _customerRepository.FindByNameAsync(context.Message.Name);

            if (existing != null)
            {
                throw new CustomerNameAlreadyExistsException();
            }

            var customer = _mapper.Map<Customer>(context.Message);

            await _customerRepository.InsertAsync(customer);

            return new CreateCustomerResponse { CustomerId = customer.Id };
        }
    }
}