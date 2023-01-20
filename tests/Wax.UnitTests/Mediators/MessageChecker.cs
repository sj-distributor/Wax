using System.Linq;
using Mediator.Net.Contracts;
using Shouldly;
using Wax.Core;
using Wax.Core.Middlewares.FluentMessageValidator;
using Wax.Messages;
using Xunit;

namespace Wax.UnitTests.Mediators;

public class MessageChecker
{
    [Fact]
    public void ShouldAllCommandHandlersHaveUniformResponses()
    {
        var _ = typeof(IUniformResponse).Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IRequest)) || t.IsAssignableTo(typeof(ICommand)))
            .Where(t => t.IsClass)
            .ToList();
    }

    [Fact]
    public void ShouldAllMessageHaveValidators()
    {
        var messageTypes = typeof(IUniformResponse).Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IRequest)) || t.IsAssignableTo(typeof(ICommand)))
            .Where(t => t.IsClass)
            .ToList();

        var validatorTypes = typeof(ApplicationModule).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Where(t => t.BaseType is { IsGenericType: true } &&
                        t.BaseType.GetGenericTypeDefinition() == typeof(FluentMessageValidator<>))
            .ToList();

        var noValidatorMessageTypes = (from messageType in messageTypes
            let hasValidator = validatorTypes.Any(x => x.BaseType != null && x.BaseType.GenericTypeArguments[0] == messageType)
            where !hasValidator
            select messageType).ToList();

        noValidatorMessageTypes.ShouldBeEmpty(
            $"[{string.Join(", ", noValidatorMessageTypes.Select(t => t.FullName))}] has no validators.");
        
        messageTypes.ShouldNotBeNull();
    }
}