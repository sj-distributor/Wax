using AutoMapper;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Domain.Customers;
using Wax.Core.Services.Customers;
using Wax.Core.Services.Customers.Exceptions;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers;

public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand>
{
    private readonly IMapper _mapper;
    private readonly ICustomerChecker _checker;
    private readonly ICustomerRepository _repository;

    public UpdateCustomerCommandHandler(IMapper mapper, ICustomerChecker checker, ICustomerRepository repository)
    {
        _mapper = mapper;
        _checker = checker;
        _repository = repository;
    }

    public async Task Handle(IReceiveContext<UpdateCustomerCommand> context, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(context.Message.CustomerId).ConfigureAwait(false);

        if (customer.Name != context.Message.Name)
        {
            if (!await _checker.CheckIsUniqueNameAsync(context.Message.Name).ConfigureAwait(false))
            {
                throw new CustomerNameAlreadyExistsException();
            }
        }

        _mapper.Map(context.Message, customer);

        await _repository.UpdateAsync(customer).ConfigureAwait(false);
    }
}