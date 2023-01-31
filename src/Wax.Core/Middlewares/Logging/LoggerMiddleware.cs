using Mediator.Net;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Serilog;

namespace Wax.Core.Middlewares.Logging;

public static class LoggerMiddleware
{
    public static void UseLogger<TContext>(this IPipeConfigurator<TContext> configurator,
        ILogger logger = null)
        where TContext : IContext<IMessage>
    {
        if (logger == null && configurator.DependencyScope == null)
        {
            throw new DependencyScopeNotConfiguredException(
                $"{nameof(logger)} is not provided and IDependencyScope is not configured, Please ensure {nameof(logger)} is registered properly if you are using IoC container, otherwise please pass {nameof(logger)} as parameter");
        }

        logger ??= configurator.DependencyScope.Resolve<ILogger>();

        configurator.AddPipeSpecification(new LoggerSpecification<TContext>(logger));
    }
}