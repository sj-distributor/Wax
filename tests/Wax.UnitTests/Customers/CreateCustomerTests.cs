using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using NSubstitute;
using Shouldly;
using Wax.Core.Domain.Customers;
using Wax.Core.Handlers.CommandHandlers.Customers;
using Wax.Core.Profiles;
using Wax.Core.Services.Customers;
using Wax.Core.Services.Customers.Exceptions;
using Wax.Messages.Commands.Customers;
using Xunit;

namespace Wax.UnitTests.Customers;

public class CreateCustomerTests
{
    private readonly ICustomerDataProvider _mockCustomerDataProvider;
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerTests()
    {
        _mockCustomerDataProvider = Substitute.For<ICustomerDataProvider>();

        _handler = new CreateCustomerCommandHandler(
            TestUtil.CreateMapper(new CustomerProfile()),
            _mockCustomerDataProvider);
    }

    [Fact]
    public async Task CannotCreateCustomerWhenNameAlreadyExists()
    {
        var command = new CreateCustomerCommand
        {
            Name = "microsoft"
        };

        _mockCustomerDataProvider.CheckIsUniqueNameAsync(command.Name).Returns(false);

        await Should.ThrowAsync<CustomerNameAlreadyExistsException>(async () =>
            await _handler.Handle(new ReceiveContext<CreateCustomerCommand>(command), CancellationToken.None));
    }

    [Fact]
    public async Task ShouldCallAddCustomer()
    {
        var callCounter = 0;

        var command = new CreateCustomerCommand
        {
            Name = "microsoft",
            Contact = "+861306888888"
        };

        _mockCustomerDataProvider.CheckIsUniqueNameAsync(command.Name).Returns(true);
        _mockCustomerDataProvider.When(x => x.AddAsync(Arg.Any<Customer>())).Do(_ => callCounter++);

        await _handler.Handle(new ReceiveContext<CreateCustomerCommand>(command), CancellationToken.None);
        callCounter.ShouldBe(1);
    }
}