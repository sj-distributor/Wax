namespace Wax.UnitTests.Customers;

public class CreateCustomerTests : CustomerTestFixture
{
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerTests()
    {
        _handler = new CreateCustomerCommandHandler(MockCustomerService.Object);
    }

    [Fact]
    public async Task ShouldNotCreateCustomerWhenNameAlreadyExists()
    {
        MockCustomerService.Setup(x => x.IsUniqueCustomerNameAsync(It.IsAny<string>())).ReturnsAsync(false);

        await Should.ThrowAsync<WaxException>(async () =>
            await _handler.Handle(new ReceiveContext<CreateCustomerCommand>(new CreateCustomerCommand()),
                CancellationToken.None));
    }

    [Theory,AutoData]
    public async Task ShouldCallInsertCustomer(CreateCustomerCommand command)
    {
        MockCustomerService.Setup(x => x.IsUniqueCustomerNameAsync(It.IsAny<string>())).ReturnsAsync(true);

        await _handler.Handle(new ReceiveContext<CreateCustomerCommand>(command), CancellationToken.None);

        MockCustomerService.Verify(x => x.InsertCustomerAsync(It.IsAny<Customer>()), Times.Once);
    }
}