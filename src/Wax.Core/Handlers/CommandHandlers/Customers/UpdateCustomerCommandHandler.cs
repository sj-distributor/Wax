using AutoMapper;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Domain.Customers;
using Wax.Core.Domain.Customers.Exceptions;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers;

public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand>
{
    private readonly IMapper _mapper;
    private readonly ICustomerRepository _repository;

    public UpdateCustomerCommandHandler(IMapper mapper, ICustomerRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task Handle(IReceiveContext<UpdateCustomerCommand> context, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(context.Message.CustomerId, cancellationToken)
            .ConfigureAwait(false);

        if (customer.Name != context.Message.Name)
        {
            if (!await _repository.CheckIsUniqueNameAsync(context.Message.Name, cancellationToken)
                    .ConfigureAwait(false))
            {
                throw new CustomerNameAlreadyExistsException();
            }
        }

        _mapper.Map(context.Message, customer);

        await _repository.UpdateAsync(customer, cancellationToken).ConfigureAwait(false);
    }
}