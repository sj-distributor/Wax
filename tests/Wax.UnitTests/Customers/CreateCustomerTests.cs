using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Mediator.Net.Context;
using NSubstitute;
using Shouldly;
using Wax.Core;
using Wax.Core.Domain;
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
    private readonly ICustomerChecker _checker;
    private readonly ICustomerRepository _repository;
    private readonly CreateCustomerCommandHandler _handler;

    public CreateCustomerTests()
    {
        var mapper = new MapperConfiguration(x => x.AddProfile(new CustomerProfile())).CreateMapper();

        _checker = Substitute.For<ICustomerChecker>();
        _repository = Substitute.For<ICustomerRepository>();

        _handler = new CreateCustomerCommandHandler(mapper, _checker, _repository);
    }

    [Fact]
    public async Task CannotCreateCustomerWhenNameAlreadyExists()
    {
        var command = new CreateCustomerCommand
        {
            Name = "microsoft"
        };

        _checker.CheckIsUniqueNameAsync(command.Name).Returns(false);

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

        _checker.CheckIsUniqueNameAsync(command.Name).Returns(true);
        _repository.When(x => x.InsertAsync(Arg.Any<Customer>())).Do(_ => callCounter++);

        await _handler.Handle(new ReceiveContext<CreateCustomerCommand>(command), CancellationToken.None);
        callCounter.ShouldBe(1);
    }
}