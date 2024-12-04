namespace Wax.UnitTests.Customers;

public class UpdateCustomerTests : CustomerTestFixture
{
    private readonly UpdateCustomerCommandHandler _handler;

    public UpdateCustomerTests()
    {
        _handler = new UpdateCustomerCommandHandler(MockCustomerService.Object);
    }

    [Theory, AutoData]
    public async Task ShouldNotUpdateCustomerWhenNameAlreadyExists(Customer customer, UpdateCustomerCommand command)
    {
        command.CustomerId = customer.Id;
        
        MockCustomerService.Setup(x => x.GetCustomerByIdAsync(customer.Id)).ReturnsAsync(customer);
        MockCustomerService.Setup(x => x.IsUniqueCustomerNameAsync(command.Name)).ReturnsAsync(false);

        await Should.ThrowAsync<WaxException>(async () =>
            await _handler.Handle(new ReceiveContext<UpdateCustomerCommand>(command), CancellationToken.None));
    }

    [Theory, AutoData]
    public async Task ShouldUpdateCustomer(Customer customer, UpdateCustomerCommand command)
    {
        command.CustomerId = customer.Id;
        
        MockCustomerService.Setup(x => x.GetCustomerByIdAsync(customer.Id)).ReturnsAsync(customer);
        MockCustomerService.Setup(x => x.IsUniqueCustomerNameAsync(command.Name)).ReturnsAsync(true);

        await _handler.Handle(new ReceiveContext<UpdateCustomerCommand>(command), CancellationToken.None);

        customer.Name.ShouldBe(command.Name);
        customer.Contact.ShouldBe(command.Contact);

        MockCustomerService.Verify(x => x.UpdateCustomerAsync(customer), Times.Once);
    }
}