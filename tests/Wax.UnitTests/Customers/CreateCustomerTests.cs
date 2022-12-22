using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using NSubstitute;
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
        _handler = new CreateCustomerCommandHandler(Mapper, Repository);
    }

    [Fact]
    public async Task CannotCreateCustomerWhenNameAlreadyExists()
    {
        var command = new CreateCustomerCommand
        {
            Name = "microsoft"
        };
        
        Repository.CheckIsUniqueNameAsync(command.Name).Returns(false);

        await Should.ThrowAsync<CustomerNameAlreadyExistsException>(async () =>
            await _handler.Handle(new ReceiveContext<CreateCustomerCommand>(command), CancellationToken.None));
    }

    [Fact]
    public async Task CanInsertCustomer()
    {
        var command = new CreateCustomerCommand
        {
            Name = "microsoft",
            Contact = "+861306888888"
        };
        
        Repository.CheckIsUniqueNameAsync(command.Name).Returns(true);

        await _handler.Handle(new ReceiveContext<CreateCustomerCommand>(command), CancellationToken.None);

        await Repository.Received().InsertAsync(Arg.Any<Customer>());
    }
}