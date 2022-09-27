using System;
using System.Threading.Tasks;
using Autofac;
using NSubstitute;
using Serilog;
using Wax.Core;
using Wax.Core.Services.Identity;

namespace Wax.IntegrationTests;

public class TestBaseFixture
{
    private readonly ILifetimeScope _scope;
    
    public TestBaseFixture()
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
}

public class IntegrationTestUser : ICurrentUser
{
    public string Id => "__integration_test_user";
}