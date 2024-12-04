using AutoMapper;
using Wax.Core.Mappings;
using Wax.Infrastructure.Authentication;
using Wax.Infrastructure.Logging;
using Wax.Infrastructure.Mediation;
using Wax.Infrastructure.Settings;

namespace Wax.Infrastructure;

public class ApplicationStartup
{
    public static void Initialize(
        ContainerBuilder builder,
        string connectionString,
        ILogger logger,
        IUserContext userContext,
        IConfiguration configuration)
    {
        var assemblies = new[] { typeof(IUserContext).Assembly, typeof(ApplicationStartup).Assembly };

        builder.RegisterModule(new LoggingModule(logger));
        builder.RegisterModule(new AuthenticationModule(userContext));
        builder.RegisterModule(new SettingModule(configuration, assemblies));
        builder.RegisterModule(new MediatorModule(assemblies));
        builder.RegisterModule(new PersistenceModule(connectionString));
        
        builder.RegisterAutoMapper(assemblies: assemblies);
        RegisterDependency(builder);
        
        builder.RegisterBuildCallback(container =>
        {
            var mapper = container.Resolve<IMapper>();
            AutoMapperConfiguration.Init(mapper.ConfigurationProvider);
        });
    }
    
    private static void RegisterDependency(ContainerBuilder builder)
    {
        foreach (var type in typeof(IDependency).Assembly.GetTypes()
                     .Where(type => type.IsClass && typeof(IDependency).IsAssignableFrom(type)))
        {
            if (typeof(IScopedDependency).IsAssignableFrom(type))
                builder.RegisterType(type).AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
            else if (typeof(ISingletonDependency).IsAssignableFrom(type))
                builder.RegisterType(type).AsSelf().AsImplementedInterfaces().SingleInstance();
            else if (typeof(ITransientDependency).IsAssignableFrom(type))
                builder.RegisterType(type).AsSelf().AsImplementedInterfaces().InstancePerDependency();
            else
                builder.RegisterType(type).AsSelf().AsImplementedInterfaces();
        }
    }
}