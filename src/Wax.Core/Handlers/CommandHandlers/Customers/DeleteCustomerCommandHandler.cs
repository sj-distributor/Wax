using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Wax.Core.Domain.Customers;
using Wax.Messages;
using Wax.Messages.Commands.Customers;

namespace Wax.Core.Handlers.CommandHandlers.Customers;

public class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand, UniformResponse>
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<UniformResponse> Handle(IReceiveContext<DeleteCustomerCommand> context,
        CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(context.Message.CustomerId, cancellationToken);

        await _customerRepository.DeleteAsync(customer, cancellationToken);

        return UniformResponse.Succeed();
    }
}