using AutoMapper;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Data;
using Wax.Core.Domain.Customers.Exceptions;
using Wax.Core.Repositories;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers;

public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryProvider _provider;

    public UpdateCustomerCommandHandler(IMapper mapper, IRepositoryProvider provider)
    {
        _mapper = mapper;
        _provider = provider;
    }

    public async Task Handle(IReceiveContext<UpdateCustomerCommand> context, CancellationToken cancellationToken)
    {
        var customer = await _provider.Customers.GetByIdAsync(context.Message.CustomerId, cancellationToken);

        if (customer.Name != context.Message.Name)
        {
            var existing = await _provider.Customers.FindByNameAsync(context.Message.Name);

            if (existing != null)
            {
                throw new CustomerNameAlreadyExistsException();
            }
        }

        _mapper.Map(context.Message, customer);

        await _provider.Customers.UpdateAsync(customer, cancellationToken);
    }
}