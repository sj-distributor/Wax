namespace Wax.Core.Commands.Customers;

public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand>
{
    private readonly ICustomerService _customerService;

    public UpdateCustomerCommandHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task Handle(IReceiveContext<UpdateCustomerCommand> context, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetCustomerByIdAsync(context.Message.CustomerId);

        if (customer.Name != context.Message.Name)
        {
            if (!await _customerService.IsUniqueCustomerNameAsync(context.Message.Name))
            {
                throw new WaxException("Customer with this name already exists.");
            }
            
            customer.Name = context.Message.Name;
        }

        customer.Address = context.Message.Address;
        customer.Contact = context.Message.Contact;

        await _customerService.UpdateCustomerAsync(customer);
    }
}