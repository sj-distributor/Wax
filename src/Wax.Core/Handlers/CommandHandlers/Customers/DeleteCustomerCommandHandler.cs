using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Data;
using Wax.Core.Repositories;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers;

public class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand>
{
    private readonly IRepositoryProvider _provider;

    public DeleteCustomerCommandHandler(IRepositoryProvider provider)
    {
        _provider = provider;
    }

    public async Task Handle(IReceiveContext<DeleteCustomerCommand> context, CancellationToken cancellationToken)
    {
        var customer = await _provider.Customers.GetByIdAsync(context.Message.CustomerId, cancellationToken);

        await _provider.Customers.DeleteAsync(customer, cancellationToken);
    }
}