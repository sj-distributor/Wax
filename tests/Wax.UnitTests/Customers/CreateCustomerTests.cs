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
        _handler = new CreateCustomerCommandHandler(Mapper, CustomerRepository);
    }

    [Fact]
    public async Task ShouldNotCreateCustomerWhenNameAlreadyExists()
    {
        var command = new CreateCustomerCommand
        {
            Name = "microsoft"
        };

        CustomerRepository.IsUniqueAsync(command.Name).Returns(false);

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

        CustomerRepository.IsUniqueAsync(command.Name).Returns(true);

        await _handler.Handle(new ReceiveContext<CreateCustomerCommand>(command), CancellationToken.None);

        await CustomerRepository.Received().InsertAsync(Arg.Any<Customer>());
    }
}