using AutoMapper;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Domain.Customers.Exceptions;
using Wax.Core.Repositories;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers;

public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand>
{
    private readonly IMapper _mapper;
    private readonly IRepository _repository;

    public UpdateCustomerCommandHandler(IMapper mapper, IRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task Handle(IReceiveContext<UpdateCustomerCommand> context, CancellationToken cancellationToken)
    {
        var customer = await _repository.Customers.GetByIdAsync(context.Message.CustomerId, cancellationToken)
            .ConfigureAwait(false);

        if (customer.Name != context.Message.Name)
        {
            if (!await _repository.Customers.CheckIsUniqueNameAsync(context.Message.Name, cancellationToken)
                    .ConfigureAwait(false))
            {
                throw new CustomerNameAlreadyExistsException();
            }
        }

        _mapper.Map(context.Message, customer);

        await _repository.Customers.UpdateAsync(customer, cancellationToken).ConfigureAwait(false);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}