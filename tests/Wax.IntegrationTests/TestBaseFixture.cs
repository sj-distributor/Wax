using System;
using System.Threading.Tasks;
using Autofac;
using NSubstitute;
using Serilog;
using Wax.Core;
using Wax.Core.Data;
using Wax.Core.Services.Identity;
using Xunit;

namespace Wax.IntegrationTests;

public class TestBaseFixture : IAsyncLifetime
{
    private readonly ILifetimeScope _scope;

    protected TestBaseFixture()
    {
        var containerBuilder = new ContainerBuilder();
        var logger = Substitute.For<ILogger>();

        containerBuilder.RegisterModule(new ApplicationModule(logger, new IntegrationTestUser(), "",
            typeof(TestBaseFixture).Assembly));

        _scope = containerBuilder.Build();
    }

    protected async Task Run<T>(Func<T, Task> action, Action<ContainerBuilder> extraRegistration = null)
    {
        var dependency = extraRegistration != null
            ? _scope.BeginLifetimeScope(extraRegistration).Resolve<T>()
            : _scope.BeginLifetimeScope().Resolve<T>();
        await action(dependency);
    }

    protected async Task<TR> Run<T, TR>(Func<T, Task<TR>> action, Action<ContainerBuilder> extraRegistration = null)
    {
        var dependency = extraRegistration != null
            ? _scope.BeginLifetimeScope(extraRegistration).Resolve<T>()
            : _scope.BeginLifetimeScope().Resolve<T>();

        return await action(dependency);
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        var dbContext = _scope.Resolve<ApplicationDbContext>();

        await dbContext.Database.EnsureDeletedAsync();
    }
}

public class IntegrationTestUser : ICurrentUser
{
    public string Id => "__integration_test_user";
}