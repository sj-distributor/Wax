using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Data;
using Wax.Core.Repositories;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers;

public class DeleteCustomerCommandHandler: ICommandHandler<DeleteCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    
    public async Task Handle(IReceiveContext<DeleteCustomerCommand> context, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(context.Message.CustomerId);

        await _customerRepository.DeleteAsync(customer);
    }
}