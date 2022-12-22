using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Mediator.Net.Context;
using NSubstitute;
using Shouldly;
using Wax.Core.Domain.Customers;
using Wax.Core.Domain.Customers.Exceptions;
using Wax.Core.Handlers.CommandHandlers.Customers;
using Wax.Core.Profiles;
using Wax.Messages.Commands.Customers;
using Xunit;

namespace Wax.UnitTests.Customers;

public class UpdateCustomerTests : CustomerTestFixture
{
    private readonly UpdateCustomerCommandHandler _handler;

    public UpdateCustomerTests()
    {
        _handler = new UpdateCustomerCommandHandler(Mapper, Repository);
    }

    [Fact]
    public async Task CannotUpdateCustomerWhenNameAlreadyExists()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = Guid.NewGuid(),
            Name = "microsoft"
        };

        Repository.GetByIdAsync(command.CustomerId)
            .Returns(new Customer { Id = command.CustomerId, Name = "google" });

        Repository.CheckIsUniqueNameAsync(command.Name).Returns(false);

        await Should.ThrowAsync<CustomerNameAlreadyExistsException>(async () =>
            await _handler.Handle(new ReceiveContext<UpdateCustomerCommand>(command), CancellationToken.None));
    }

    [Fact]
    public async Task CanUpdateCustomer()
    {
        var customer = new Customer { Id = Guid.NewGuid(), Name = "google" };

        var command = new UpdateCustomerCommand
        {
            CustomerId = customer.Id,
            Name = "google",
            Contact = "+861306888888"
        };

        Repository.GetByIdAsync(command.CustomerId).Returns(customer);
        Repository.CheckIsUniqueNameAsync(command.Name).Returns(true);

        await _handler.Handle(new ReceiveContext<UpdateCustomerCommand>(command), CancellationToken.None);

        customer.Name.ShouldBe(command.Name);
        customer.Contact.ShouldBe(command.Contact);

        await Repository.Received().UpdateAsync(Arg.Any<Customer>());
    }
}