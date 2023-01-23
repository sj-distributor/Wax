using System;
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
    public void ShouldAllHandlersHaveUniformResponses()
    {
        var assemblyTypes = typeof(ApplicationModule).Assembly.GetTypes();

        var notImplementedIUniformResponseHandlers = assemblyTypes.Where(t => !t.IsAbstract && !t.IsInterface)
            .Where(t => IsAssignableToGenericType(t, typeof(ICommandHandler<>))).ToList();

        var handlers = assemblyTypes.Where(t => !t.IsAbstract && !t.IsInterface)
            .Where(t => IsAssignableToGenericType(t, typeof(IRequestHandler<,>)) ||
                        IsAssignableToGenericType(t, typeof(ICommandHandler<,>))).ToList();

        notImplementedIUniformResponseHandlers.AddRange(from handler in handlers
            let imp = handler.GetInterface(typeof(IRequestHandler<,>).Name) ??
                      handler.GetInterface(typeof(ICommandHandler<,>).Name)
            where !imp.GetGenericArguments()[1].IsAssignableTo(typeof(IUniformResponse))
            select handler);

        notImplementedIUniformResponseHandlers.ShouldBeEmpty(
            $"[{string.Join(", ", notImplementedIUniformResponseHandlers.Select(t => t.FullName))}] has not implement IUniformResponse.");
    }

    [Fact]
    public void ShouldAllMessageHaveValidators()
    {
        var messageTypes = typeof(IUniformResponse).Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IRequest)) || t.IsAssignableTo(typeof(ICommand)))
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .ToList();

        var validatorTypes = typeof(ApplicationModule).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Where(t => t.BaseType is {IsGenericType: true} &&
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

    private static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        var interfaceTypes = givenType.GetInterfaces();

        if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
        {
            return true;
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            return true;

        var baseType = givenType.BaseType;
        return baseType != null && IsAssignableToGenericType(baseType, genericType);
    }
}