using System;
using System.Threading.Tasks;
using Autofac;
using Mediator.Net;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Wax.Core.Domain.Customers;
using Wax.Messages;
using Wax.Messages.Commands.Customers;
using Wax.Messages.Enums;
using Xunit;

namespace Wax.IntegrationTests.Mediators;

public class ExceptionUniformResponseMiddlewareTests : TestBaseFixture
{
    [Fact]
    public async Task ShouldReturnBadRequestResponse()
    {
        await Run<IMediator>(async mediator =>
        {
            var response = await mediator.SendAsync<UpdateCustomerCommand, IUniformResponse>(
                new UpdateCustomerCommand
                {
                    CustomerId = Guid.NewGuid(),
                    Name = ""
                });

            response.Success.ShouldBeFalse();
            response.Error.Code.ShouldBe(ErrorCode.BadRequest);
        });
    }
    
    [Fact]
    public async Task ShouldReturnNotFoundResponse()
    {
        await Run<IMediator>(async mediator =>
        {
            var response = await mediator.SendAsync<UpdateCustomerCommand, IUniformResponse>(
                new UpdateCustomerCommand
                {
                    CustomerId = Guid.NewGuid(),
                    Name = "microsoft"
                });

            response.Success.ShouldBeFalse();
            response.Error.Code.ShouldBe(ErrorCode.NotFound);
        });
    }

    [Fact]
    public async Task ShouldReturnUndefinedResponse()
    {
        await Run<IMediator>(async mediator =>
        {
            var response = await mediator.SendAsync<UpdateCustomerCommand, IUniformResponse>(
                new UpdateCustomerCommand
                {
                    CustomerId = Guid.NewGuid(),
                    Name = "microsoft"
                });

            response.Success.ShouldBeFalse();
            response.Error.Code.ShouldBe(ErrorCode.Undefined);
        }, builder =>
        {
            var repository = Substitute.For<ICustomerRepository>();
            repository.GetByIdAsync(Arg.Any<Guid>()).ThrowsAsync(new ArgumentNullException());
            builder.RegisterInstance(repository).AsImplementedInterfaces();
        });
    }
}