using System.Linq;
using Mediator.Net.Contracts;
using Shouldly;
using Wax.Core;
using Wax.Core.Processing.FluentMessageValidator;
using Wax.Messages.Commands.Customers;
using Xunit;

namespace Wax.UnitTests.Mediators;

public class MessageChecker
{
    [Fact]
    public void ShouldAllMessageHaveValidators()
    {
        var messageTypes = typeof(CreateCustomerCommand).Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IRequest)) || t.IsAssignableTo(typeof(ICommand)))
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .ToList();

        var validatorTypes = typeof(ApplicationModule).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Where(t => t.BaseType is { IsGenericType: true } &&
                        t.BaseType.GetGenericTypeDefinition() == typeof(FluentMessageValidator<>)).ToList();

        var noValidatorMessageTypes = (from messageType in messageTypes
            let hasValidator =
                validatorTypes.Any(x => x.BaseType != null && x.BaseType.GenericTypeArguments[0] == messageType)
            where !hasValidator
            select messageType).ToList();

        noValidatorMessageTypes.ShouldBeEmpty(
            $"[{string.Join(", ", noValidatorMessageTypes.Select(t => t.FullName))}] has no validators.");

        messageTypes.ShouldNotBeNull();
    }
}