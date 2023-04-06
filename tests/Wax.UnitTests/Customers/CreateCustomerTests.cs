using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;
using Wax.Core.Domain.Customers;
using Wax.Core.Domain.Customers.Exceptions;
using Wax.Core.Handlers.CommandHandlers.Customers;
using Wax.Messages.Commands.Customers;
using Xunit;

namespace Wax.UnitTests.Customers;

public class CreateCustomerTests : CustomerTestFixture
{
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerTests()
    {
        _handler = new CreateCustomerCommandHandler(Mapper, Customers);
    }

    [Fact]
    public async Task ShouldNotCreateCustomerWhenNameAlreadyExists()
    {
        var command = new CreateCustomerCommand
        {
            Name = "microsoft"
        };

        Customers.FindByNameAsync(command.Name).Returns(new Customer { Name = "google" });

        await Should.ThrowAsync<CustomerNameAlreadyExistsException>(async () =>
            await _handler.Handle(new ReceiveContext<CreateCustomerCommand>(command), CancellationToken.None));
    }

    [Fact]
    public async Task ShouldCallInsert()
    {
        var command = new CreateCustomerCommand
        {
            Name = "microsoft",
            Contact = "+861306888888"
        };

        Customers.FindByNameAsync(command.Name).ReturnsNull();

        await _handler.Handle(new ReceiveContext<CreateCustomerCommand>(command), CancellationToken.None);

        await Customers.Received().InsertAsync(Arg.Any<Customer>());
    }
}