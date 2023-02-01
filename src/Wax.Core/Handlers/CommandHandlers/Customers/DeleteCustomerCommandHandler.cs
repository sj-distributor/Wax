using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Repositories;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers;

public class DeleteCustomerCommandHandler: ICommandHandler<DeleteCustomerCommand>
{
    private readonly IRepository _repository;

    public DeleteCustomerCommandHandler(IRepository repository)
    {
        _repository = repository;
    }
    
    public async Task Handle(IReceiveContext<DeleteCustomerCommand> context, CancellationToken cancellationToken)
    {
        var customer = await _repository.Customers.GetByIdAsync(context.Message.CustomerId, cancellationToken);

        await _repository.Customers.DeleteAsync(customer, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}