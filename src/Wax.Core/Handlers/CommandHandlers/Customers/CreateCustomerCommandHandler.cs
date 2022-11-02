using AutoMapper;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Domain.Customers;
using Wax.Core.Services.Customers;
using Wax.Core.Services.Customers.Exceptions;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CreateCustomerResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerChecker _checker;
        private readonly ICustomerRepository _repository;

        public CreateCustomerCommandHandler(IMapper mapper, ICustomerChecker checker, ICustomerRepository repository)
        {
            _mapper = mapper;
            _checker = checker;
            _repository = repository;
        }

        public async Task<CreateCustomerResponse> Handle(IReceiveContext<CreateCustomerCommand> context,
            CancellationToken cancellationToken)
        {
            if (!await _checker.CheckIsUniqueNameAsync(context.Message.Name))
            {
                throw new CustomerNameAlreadyExistsException();
            }

            var customer = _mapper.Map<Customer>(context.Message);

            await _repository.InsertAsync(customer).ConfigureAwait(false);

            return new CreateCustomerResponse {CustomerId = customer.Id};
        }
    }
}