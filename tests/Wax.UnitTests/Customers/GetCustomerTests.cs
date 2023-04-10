using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Net.Context;
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
        _handler = new GetCustomerRequestHandler(Mapper, Customers);
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

        Customers.GetByIdAsync(customer.Id).Returns(Task.FromResult(customer));

        var response = await _handler.Handle(new ReceiveContext<GetCustomerRequest>(
            new GetCustomerRequest
            {
                CustomerId = customer.Id
            }), CancellationToken.None);

        response.Customer.ShouldNotBeNull();
        response.Customer.Id.ShouldBe(customer.Id);
        response.Customer.Name.ShouldBe(customer.Name);
        response.Customer.Address.ShouldBe(customer.Address);
    }
}