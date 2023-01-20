using Mediator.Net;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Wax.Core.Middlewares.FluentMessageValidator;

public static class MessageValidatorMiddleware
{
    public static void UseMessageValidator<TContext>(this IPipeConfigurator<TContext> configurator,
        IEnumerable<IFluentMessageValidator> messageValidators = null)
        where TContext : IContext<IMessage>
    {
        if (messageValidators == null && configurator.DependencyScope == null)
        {
            throw new DependencyScopeNotConfiguredException(
                $"{nameof(messageValidators)} is not provided and IDependencyScope is not configured, Please ensure {nameof(messageValidators)} is registered properly if you are using IoC container, otherwise please pass {nameof(messageValidators)} as parameter");
        }

        messageValidators ??= configurator.DependencyScope.Resolve<IEnumerable<IFluentMessageValidator>>();

        configurator.AddPipeSpecification(new MessageValidatorSpecification<TContext>(messageValidators));
    }
}