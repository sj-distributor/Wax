using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Repositories;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers;

public class DeleteCustomerCommandHandler: ICommandHandler<DeleteCustomerCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(IReceiveContext<DeleteCustomerCommand> context, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(context.Message.CustomerId, cancellationToken);

        await _unitOfWork.Customers.DeleteAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}