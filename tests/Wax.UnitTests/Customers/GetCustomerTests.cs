using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
using MockQueryable.NSubstitute;
using NSubstitute;
using Shouldly;
using Wax.Core.Domain.Customers;
using Wax.Core.Handlers.RequestHandlers.Customers;
using Wax.Messages.Requests.Customers;
using Xunit;

namespace Wax.UnitTests.Customers;

public class GetCustomerTests : CustomerTestFixture
{
    private readonly GetCustomerRequestHandler _handler;

    public GetCustomerTests()
    {
        _handler = new GetCustomerRequestHandler(RepositoryProvider);
    }


    [Fact]
    public async Task ShouldGetCustomer()
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "microsoft",
            Address = "HuiZhou, Guangdong Province, China",
            Contact = "+861306888888"
        };

        Customers.Table.Returns(new List<Customer> { customer }.BuildMock());

        var response = await _handler.Handle(new ReceiveContext<GetCustomerRequest>(
            new GetCustomerRequest
            {
                CustomerId = customer.Id
            }), CancellationToken.None);

        response.Data.ShouldNotBeNull();
        response.Data.Id.ShouldBe(customer.Id);
        response.Data.Name.ShouldBe(customer.Name);
        response.Data.Address.ShouldBe(customer.Address);
    }
}