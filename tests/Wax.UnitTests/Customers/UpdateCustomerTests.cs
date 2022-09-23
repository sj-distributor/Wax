using System;
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

public class UpdateCustomerTests
{
    private readonly ICustomerDataProvider _mockCustomerDataProvider;
    private readonly UpdateCustomerCommandHandler _handler;

    public UpdateCustomerTests()
    {
        _mockCustomerDataProvider = Substitute.For<ICustomerDataProvider>();

        _handler = new UpdateCustomerCommandHandler(
            TestUtil.CreateMapper(new CustomerProfile()),
            _mockCustomerDataProvider);
    }

    [Fact]
    public async Task CannotUpdateCustomerWhenNameAlreadyExists()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = Guid.NewGuid(),
            Name = "microsoft"
        };

        _mockCustomerDataProvider.GetByIdAsync(command.CustomerId)
            .Returns(new Customer { Id = command.CustomerId, Name = "google" });

        _mockCustomerDataProvider.CheckIsUniqueNameAsync(command.Name).Returns(false);

        await Should.ThrowAsync<CustomerNameAlreadyExistsException>(async () =>
            await _handler.Handle(new ReceiveContext<UpdateCustomerCommand>(command), CancellationToken.None));
    }

    [Fact]
    public async Task ShouldCallUpdateCustomer()
    {
        var callCounter = 0;
    
        var customer = new Customer { Id = Guid.NewGuid(), Name = "google" };
        
        var command = new UpdateCustomerCommand
        {
            CustomerId = customer.Id,
            Name = "google",
            Contact = "+861306888888"
        };

        _mockCustomerDataProvider.GetByIdAsync(command.CustomerId).Returns(customer);
        _mockCustomerDataProvider.CheckIsUniqueNameAsync(command.Name).Returns(true);
        _mockCustomerDataProvider.When(x => x.UpdateAsync(Arg.Any<Customer>())).Do(_ => callCounter++);
    
        await _handler.Handle(new ReceiveContext<UpdateCustomerCommand>(command), CancellationToken.None);
        callCounter.ShouldBe(1);

        customer.Name.ShouldBe(command.Name);
        customer.Contact.ShouldBe(command.Contact);
    }
}