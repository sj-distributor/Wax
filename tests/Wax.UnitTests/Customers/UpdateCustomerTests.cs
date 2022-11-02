using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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
    private readonly ICustomerChecker _checker;
    private readonly ICustomerRepository _repository;
    private readonly UpdateCustomerCommandHandler _handler;

    public UpdateCustomerTests()
    {
        var mapper = new MapperConfiguration(x => x.AddProfile(new CustomerProfile())).CreateMapper();
            
        _checker = Substitute.For<ICustomerChecker>();
        _repository = Substitute.For<ICustomerRepository>();


        _handler = new UpdateCustomerCommandHandler(mapper, _checker, _repository);
    }

    [Fact]
    public async Task CannotUpdateCustomerWhenNameAlreadyExists()
    {
        var command = new UpdateCustomerCommand
        {
            CustomerId = Guid.NewGuid(),
            Name = "microsoft"
        };

        _repository.GetByIdAsync(command.CustomerId)
            .Returns(new Customer { Id = command.CustomerId, Name = "google" });

        _checker.CheckIsUniqueNameAsync(command.Name).Returns(false);

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

        _repository.GetByIdAsync(command.CustomerId).Returns(customer);
        _checker.CheckIsUniqueNameAsync(command.Name).Returns(true);
        _repository.When(x => x.UpdateAsync(Arg.Any<Customer>())).Do(_ => callCounter++);
    
        await _handler.Handle(new ReceiveContext<UpdateCustomerCommand>(command), CancellationToken.None);
        callCounter.ShouldBe(1);

        customer.Name.ShouldBe(command.Name);
        customer.Contact.ShouldBe(command.Contact);
    }
}