namespace Wax.Core.Commands.Customers;

public class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand>
{
    private readonly ICustomerService _customerService;

    public DeleteCustomerCommandHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task Handle(IReceiveContext<DeleteCustomerCommand> context, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetCustomerByIdAsync(context.Message.CustomerId);

        await _customerService.DeleteCustomerAsync(customer);
    }
}