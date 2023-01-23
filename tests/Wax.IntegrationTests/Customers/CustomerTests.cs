﻿using System;
using System.Threading.Tasks;
using Mediator.Net;
using Shouldly;
using Wax.Messages;
using Wax.Messages.Commands.Customers;
using Wax.Messages.Dtos.Customers;
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

            var createCustomerResponse = await mediator.SendAsync<CreateCustomerCommand, IUniformResponse<Guid>>(
                createCustomerCommand);

            var getCustomerResponse =
                await mediator.RequestAsync<GetCustomerRequest, IUniformResponse<CustomerShortInfo>>(
                    new GetCustomerRequest
                    {
                        CustomerId = createCustomerResponse.Data
                    });

            getCustomerResponse.Data.Name.ShouldBe(createCustomerCommand.Name);
            getCustomerResponse.Data.Address.ShouldBe(createCustomerCommand.Address);
        });
    }

    [Fact]
    public async Task ShouldUpdateCustomer()
    {
        var customerId = await CreateDefaultCustomer();

        await Run<IMediator>(async mediator =>
        {
            var updateCustomerCommand = new UpdateCustomerCommand
            {
                CustomerId = customerId,
                Name = "Google",
                Address = "111 8th Ave, New York, NY 10011, United States",
                Contact = "+12125650000"
            };

            await mediator.SendAsync(updateCustomerCommand);

            var getCustomerResponse = await mediator.RequestAsync<GetCustomerRequest, IUniformResponse<CustomerShortInfo>>(
                new GetCustomerRequest
                {
                    CustomerId = updateCustomerCommand.CustomerId
                });

            getCustomerResponse.Data.Name.ShouldBe(updateCustomerCommand.Name);
            getCustomerResponse.Data.Address.ShouldBe(updateCustomerCommand.Address);
        });
    }

    [Fact]
    public async Task ShouldDeleteCustomer()
    {
        var customerId = await CreateDefaultCustomer();

        await Run<IMediator>(async mediator =>
        {
            await mediator.SendAsync(new DeleteCustomerCommand
            {
                CustomerId = customerId
            });

            var getCustomerResponse = await mediator.RequestAsync<GetCustomerRequest, IUniformResponse<CustomerShortInfo>>(
                new GetCustomerRequest
                {
                    CustomerId = customerId
                });

            getCustomerResponse.Data.ShouldBeNull();
        });
    }

    private async Task<Guid> CreateDefaultCustomer()
    {
        return await Run<IMediator, Guid>(async mediator =>
        {
            var createCustomerResponse = await mediator.SendAsync<CreateCustomerCommand, IUniformResponse<Guid>>(
                new CreateCustomerCommand
                {
                    Name = "Microsoft",
                    Address = "Microsoft Corporation One Microsoft Way Redmond, WA 98052-6399 USA",
                    Contact = "(800) 426-9400"
                });

            return createCustomerResponse.Data;
        });
    }
}