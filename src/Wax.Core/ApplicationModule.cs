using System.Reflection;
using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Mediator.Net;
using Mediator.Net.Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Wax.Core.Data;
using Wax.Core.DependencyInjection;
using Wax.Core.Middlewares.FluentMessageValidator;
using Wax.Core.Middlewares.Logging;
using Wax.Core.Middlewares.UnitOfWorks;
using Wax.Core.Repositories;
using Wax.Core.Services.Identity;
using Wax.Core.Settings;
using Module = Autofac.Module;

namespace Wax.Core
{
    public class ApplicationModule : Module
    {
        private readonly ILogger _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IConfiguration _configuration;
        private readonly Assembly[] _assemblies;

        public ApplicationModule(ILogger logger, ICurrentUser currentUser, IConfiguration configuration,
            params Assembly[] assemblies)
        {
            _logger = logger;
            _currentUser = currentUser;
            _configuration = configuration;

            _assemblies = (assemblies ?? Array.Empty<Assembly>())
                .Concat(new[] { typeof(ApplicationModule).Assembly })
                .ToArray();
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterSettings(builder);
            RegisterAutoMapper(builder);
            RegisterDependency(builder);
            RegisterDatabase(builder);
            RegisterIdentity(builder);
            RegisterLogger(builder);
            RegisterMediator(builder);
            RegisterValidator(builder);
        }
        
        private void RegisterSettings(ContainerBuilder builder)
        {
            builder.RegisterInstance(_configuration)
                .As<IConfiguration>()
                .SingleInstance();

            var settingTypes = _assemblies.SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && typeof(IConfigurationSetting).IsAssignableFrom(t))
                .ToArray();

            builder.RegisterTypes(settingTypes).AsSelf().SingleInstance();
        }

        private void RegisterAutoMapper(ContainerBuilder builder)
        {
            builder.RegisterAutoMapper(assemblies: _assemblies);
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

        private void RegisterDatabase(ContainerBuilder builder)
        {
            builder.Register(c =>
                {
                    var connectionString = _configuration.GetConnectionString("Default");

                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        //Select your database provider
                    }

                    var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                    optionBuilder.UseInMemoryDatabase("__wax_database");

                    return new ApplicationDbContext(optionBuilder.Options);
                }).AsSelf().As<DbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BasicRepository<>))
                .As(typeof(IBasicRepository<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
        }

        private void RegisterIdentity(ContainerBuilder builder)
        {
            builder.RegisterInstance(_currentUser);
        }

        private void RegisterLogger(ContainerBuilder builder)
        {
            builder.RegisterInstance(_logger)
                .As<ILogger>()
                .SingleInstance();
        }

        private void RegisterMediator(ContainerBuilder builder)
        {
            var mediatorBuilder = new MediatorBuilder();

            mediatorBuilder.RegisterHandlers(_assemblies);
            mediatorBuilder.ConfigureGlobalReceivePipe(c =>
            {
                c.UseLogger();
                c.UseMessageValidator();
                c.UseUnitOfWork();
            });

            builder.RegisterMediator(mediatorBuilder);
        }

        private void RegisterValidator(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(_assemblies)
                .Where(t => t.IsClass && typeof(IFluentMessageValidator).IsAssignableFrom(t))
                .AsSelf().AsImplementedInterfaces();
        }
    }
}