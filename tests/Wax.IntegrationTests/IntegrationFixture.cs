using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Serilog;
using Wax.Core;
using Wax.Core.Data;
using Xunit;

namespace Wax.IntegrationTests;

public class IntegrationFixture : IAsyncLifetime
{
    public readonly ILifetimeScope LifetimeScope;

    public IntegrationFixture()
    {
        var containerBuilder = new ContainerBuilder();
        var logger = Substitute.For<ILogger>();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true).Build();

        containerBuilder.RegisterModule(new ApplicationModule(logger, new IntegrationTestUser(), configuration,
            typeof(IntegrationFixture).Assembly));

        LifetimeScope = containerBuilder.Build();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        var context = LifetimeScope.Resolve<ApplicationDbContext>();
        return context.Database.EnsureDeletedAsync();
    }
}