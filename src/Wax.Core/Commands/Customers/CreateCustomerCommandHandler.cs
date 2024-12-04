namespace Wax.Core.Commands.Customers;

public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CreateCustomerResponse>
{
    private readonly ICustomerService _customerService;

    public CreateCustomerCommandHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<CreateCustomerResponse> Handle(IReceiveContext<CreateCustomerCommand> context,
        CancellationToken cancellationToken)
    {
        if (!await _customerService.IsUniqueCustomerNameAsync(context.Message.Name))
        {
            throw new WaxException("Customer with this name already exists.");
        }

        var customer = context.Message.ToEntity<Customer>();

        await _customerService.InsertCustomerAsync(customer);

        return new CreateCustomerResponse { CustomerId = customer.Id };
    }
}