using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Wax.Core.Entities.Customers;
using Wax.Core.Services.Customers;
using Wax.Core.Services.Customers.Exceptions;
using Wax.Messages.Commands.Customers;
using Xunit;

namespace Wax.UnitTests.Customers;

public class CreateCustomerTests
{
    private readonly ICustomerDataProvider _mockCustomerDataProvider;
    private readonly ICustomerService _customerService;

    public CreateCustomerTests()
    {
        _mockCustomerDataProvider = Substitute.For<ICustomerDataProvider>();
        _customerService = new CustomerService(_mockCustomerDataProvider);
    }

    [Fact]
    public async Task ThrowExWhenCustomerNameAlreadyExists()
    {
        var command = new CreateCustomerCommand
        {
            Name = "microsoft"
        };

        _mockCustomerDataProvider.GetByNameAsync(command.Name).Returns(new Customer());

        await Should.ThrowAsync<CustomerNameAlreadyExistsException>(async () =>
            await _customerService.CreateAsync(command));
    }

    [Fact]
    public async Task ShouldCreateNewCustomer()
    {
        var callCounter = 0;
        
        var command = new CreateCustomerCommand
        {
            Name = "microsoft"
        };

        _mockCustomerDataProvider.GetByNameAsync(command.Name).ReturnsNull();
        
        _mockCustomerDataProvider.When(x => x.AddAsync(Arg.Any<Customer>())).Do(_ =>
        {
            callCounter++;
        });

        var @event = await _customerService.CreateAsync(command);
        @event.Name.ShouldBe(command.Name);
       
        callCounter.ShouldBe(1);
    }
}