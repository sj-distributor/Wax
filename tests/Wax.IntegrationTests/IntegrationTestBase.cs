using System;
using System.Threading.Tasks;
using Autofac;
using Wax.Core.Data;
using Xunit;

namespace Wax.IntegrationTests;

[Collection("Sequential")]
public class IntegrationTestBase : IAsyncLifetime
{
    private readonly ILifetimeScope _lifetimeScope;

    protected IntegrationTestBase(IntegrationFixture fixture)
    {
        _lifetimeScope = fixture.LifetimeScope;
    }

    protected async Task Run<T>(Func<T, Task> action, Action<ContainerBuilder> extraRegistration = null)
    {
        var dependency = extraRegistration != null
            ? _lifetimeScope.BeginLifetimeScope(extraRegistration).Resolve<T>()
            : _lifetimeScope.BeginLifetimeScope().Resolve<T>();
        await action(dependency);
    }

    protected Task Run<T, U>(Func<T, U, Task> action, Action<ContainerBuilder> extraRegistration = null)
    {
        var lifetime = extraRegistration != null
            ? _lifetimeScope.BeginLifetimeScope(extraRegistration)
            : _lifetimeScope.BeginLifetimeScope();
        var dependency = lifetime.Resolve<T>();
        var dependency2 = lifetime.Resolve<U>();
        return action(dependency, dependency2);
    }

    protected async Task<TR> Run<T, TR>(Func<T, Task<TR>> action, Action<ContainerBuilder> extraRegistration = null)
    {
        var dependency = extraRegistration != null
            ? _lifetimeScope.BeginLifetimeScope(extraRegistration).Resolve<T>()
            : _lifetimeScope.BeginLifetimeScope().Resolve<T>();

        return await action(dependency);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        //If have a real database, only need to clean the table after each test.
        var context = _lifetimeScope.Resolve<ApplicationDbContext>();
        return context.Database.EnsureDeletedAsync();
    }
}