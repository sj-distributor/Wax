using Mediator.Net;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;
using Wax.Core.Data;

namespace Wax.Core.Middlewares.UnitOfWorks;

public static class UnitOfWorkMiddleware
{
    public static void UseUnitOfWork<TContext>(this IPipeConfigurator<TContext> configurator)
        where TContext : IContext<IMessage>
    {
        if (configurator.DependencyScope == null)
        {
            throw new DependencyScopeNotConfiguredException("IDependencyScope is not configured.");
        }

        var uow = configurator.DependencyScope.Resolve<IUnitOfWork>();

        configurator.AddPipeSpecification(new UnitOfWorkSpecification<TContext>(uow));
    }
}