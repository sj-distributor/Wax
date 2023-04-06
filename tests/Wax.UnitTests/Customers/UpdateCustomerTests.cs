using System;
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

public class UpdateCustomerTests : CustomerTestFixture
{
    private readonly UpdateCustomerCommandHandler _handler;

    public UpdateCustomerTests()
    {
        _handler = new UpdateCustomerCommandHandler(Mapper, Customers);
    }

    [Fact]
    public async Task ShouldNotUpdateCustomerWhenNameAlreadyExists()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = Guid.NewGuid(),
            Name = "microsoft"
        };

        Customers.GetByIdAsync(command.CustomerId)
            .Returns(new Customer { Id = command.CustomerId, Name = "google" });

        Customers.FindByNameAsync(command.Name).Returns(new Customer { Name = "meta" });

        await Should.ThrowAsync<CustomerNameAlreadyExistsException>(async () =>
            await _handler.Handle(new ReceiveContext<UpdateCustomerCommand>(command), CancellationToken.None));
    }

    [Fact]
    public async Task ShouldUpdateCustomer()
    {
        var customer = new Customer { Id = Guid.NewGuid(), Name = "google" };

        var command = new UpdateCustomerCommand
        {
            CustomerId = customer.Id,
            Name = "google",
            Contact = "+861306888888"
        };

        Customers.GetByIdAsync(command.CustomerId).Returns(customer);
        Customers.FindByNameAsync(command.Name).ReturnsNull();

        await _handler.Handle(new ReceiveContext<UpdateCustomerCommand>(command), CancellationToken.None);

        customer.Name.ShouldBe(command.Name);
        customer.Contact.ShouldBe(command.Contact);

        await Customers.Received().UpdateAsync(Arg.Any<Customer>());
    }
}