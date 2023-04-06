using System;
using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Serilog;
using Wax.Core;
using Wax.Core.Data;
using Xunit;

namespace Wax.IntegrationTests;

public class IntegrationFixture : IDisposable, ICollectionFixture<IntegrationFixture>
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

    public void Cleanup()
    {
        var dbContext = LifetimeScope.Resolve<ApplicationDbContext>();
        dbContext.Database.EnsureDeleted();
    }

    public void Dispose()
    {
        var dbContext = LifetimeScope.Resolve<ApplicationDbContext>();
        dbContext.Database.EnsureDeleted();
    }
}

[CollectionDefinition("Sequential")]
public class DatabaseCollection : ICollectionFixture<IntegrationFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}