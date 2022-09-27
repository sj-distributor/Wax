using System.Threading.Tasks;
using Mediator.Net;
using Shouldly;
using Wax.Messages.Commands.Customers;
using Wax.Messages.Requests.Customers;
using Xunit;

namespace Wax.IntegrationTests.Customers;

public class CustomerTests : TestBaseFixture
{
    [Fact]
    public async Task ShouldCreateNewCustomer()
    {
        await Run<IMediator>(async mediator =>
        {
            var createCustomerCommand = new CreateCustomerCommand
            {
                Name = "Microsoft",
                Address = "Microsoft Corporation One Microsoft Way Redmond, WA 98052-6399 USA",
                Contact = "(800) 426-9400"
            };

            var createCustomerResponse = await mediator.SendAsync<CreateCustomerCommand, CreateCustomerResponse>(
                createCustomerCommand);

            var getCustomerResponse = await mediator.RequestAsync<GetCustomerRequest, GetCustomerResponse>(
                new GetCustomerRequest
                {
                    CustomerId = createCustomerResponse.CustomerId
                });

            getCustomerResponse.Customer.Name.ShouldBe(createCustomerCommand.Name);
            getCustomerResponse.Customer.Address.ShouldBe(createCustomerCommand.Address);
        });
    }
}