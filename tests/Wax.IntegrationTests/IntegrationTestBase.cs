using System;
using System.Threading.Tasks;
using Autofac;
using Wax.Core.Repositories;
using Xunit;

namespace Wax.IntegrationTests;

[Collection("Sequential")]
public class IntegrationTestBase : IDisposable
{
    private readonly IntegrationFixture _fixture;

    protected IntegrationTestBase(IntegrationFixture fixture)
    {
        _fixture = fixture;
    }

    protected async Task Run<T>(Func<T, Task> action, Action<ContainerBuilder> extraRegistration = null)
    {
        var dependency = extraRegistration != null
            ? _fixture.LifetimeScope.BeginLifetimeScope(extraRegistration).Resolve<T>()
            : _fixture.LifetimeScope.BeginLifetimeScope().Resolve<T>();
        await action(dependency);
    }

    protected async Task<TR> Run<T, TR>(Func<T, Task<TR>> action, Action<ContainerBuilder> extraRegistration = null)
    {
        var dependency = extraRegistration != null
            ? _fixture.LifetimeScope.BeginLifetimeScope(extraRegistration).Resolve<T>()
            : _fixture.LifetimeScope.BeginLifetimeScope().Resolve<T>();

        return await action(dependency);
    }

    protected async Task<TR> RunWithUnitOfWork<TR>(Func<IUnitOfWork, Task<TR>> action)
    {
        var uow = _fixture.LifetimeScope.BeginLifetimeScope().Resolve<IUnitOfWork>();
        return await action(uow);
    }
    
    protected async Task RunWithUnitOfWork(Func<IUnitOfWork, Task> action)
    {
        var uow = _fixture.LifetimeScope.BeginLifetimeScope().Resolve<IUnitOfWork>();
        await action(uow);
    }

    public void Dispose()
    {
        _fixture.Cleanup();
    }
}