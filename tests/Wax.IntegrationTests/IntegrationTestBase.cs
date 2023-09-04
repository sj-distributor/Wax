using System;
using System.Threading.Tasks;
using Autofac;
using Wax.Core.Data;
using Wax.Core.Repositories;
using Xunit;

namespace Wax.IntegrationTests;

[Collection("Sequential")]
public class IntegrationTestBase : IAsyncLifetime
{
    private readonly ILifetimeScope _lifetimeScope;
    private readonly Func<Task> _resetDatabase;

    public IntegrationTestBase(IntegrationFixture fixture)
    {
        _lifetimeScope = fixture.LifetimeScope;
        _resetDatabase = fixture.ResetDatabase;
    }

    protected async Task Run<T>(Func<T, Task> action, Action<ContainerBuilder> extraRegistration = null)
    {
        var dependency = extraRegistration != null
            ? _lifetimeScope.BeginLifetimeScope(extraRegistration).Resolve<T>()
            : _lifetimeScope.BeginLifetimeScope().Resolve<T>();
        await action(dependency);
    }

    protected async Task<TR> Run<T, TR>(Func<T, Task<TR>> action, Action<ContainerBuilder> extraRegistration = null)
    {
        var dependency = extraRegistration != null
            ? _lifetimeScope.BeginLifetimeScope(extraRegistration).Resolve<T>()
            : _lifetimeScope.BeginLifetimeScope().Resolve<T>();

        return await action(dependency);
    }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _resetDatabase();
    }
}