using System.Reflection;
using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Mediator.Net;
using Mediator.Net.Autofac;
using Mediator.Net.Middlewares.Serilog;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Wax.Core.Data;
using Wax.Core.Data.Repositories;
using Wax.Core.DependencyInjection;
using Wax.Core.Domain;
using Wax.Core.Domain.Customers;
using Wax.Core.Processing.FluentMessageValidator;
using Wax.Core.Services.Identity;
using Module = Autofac.Module;

namespace Wax.Core
{
    public class ApplicationModule : Module
    {
        private readonly ILogger _logger;
        private readonly ICurrentUser _currentUser;
        private readonly string _connectionString;
        private readonly Assembly[] _assemblies;

        public ApplicationModule(ILogger logger, ICurrentUser currentUser, string connectionString,
            params Assembly[] assemblies)
        {
            _logger = logger;
            _currentUser = currentUser;
            _connectionString = connectionString;

            _assemblies = (assemblies ?? Array.Empty<Assembly>())
                .Concat(new[] {typeof(ApplicationModule).Assembly})
                .ToArray();
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterAutoMapper(builder);
            RegisterDependency(builder);
            RegisterDatabase(builder);
            RegisterIdentity(builder);
            RegisterLogger(builder);
            RegisterMediator(builder);
            RegisterValidator(builder);
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
                    if (!string.IsNullOrEmpty(_connectionString))
                    {
                        //Select your database provider
                    }

                    var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                    optionBuilder.UseInMemoryDatabase("__wax_database");

                    return new ApplicationDbContext(optionBuilder.Options);
                }).AsSelf().As<DbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EfCoreRepository<,>))
                .As(typeof(IRepository<,>))
                .InstancePerLifetimeScope();
            
            //TODO: Register all repository
            builder.RegisterType<EfCoreCustomerRepository>().As<ICustomerRepository>().InstancePerLifetimeScope();
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
                c.UseSerilog(logger: _logger);
                c.UseMessageValidator();
            });

            builder.RegisterMediator(mediatorBuilder);
        }

        private void RegisterValidator(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(_assemblies)
                .Where(t => t.IsClass && typeof(IFluentMessageValidator).IsAssignableFrom(t))
                .AsSelf().AsImplementedInterfaces();
        }

        private static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
            {
                return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            var baseType = givenType.BaseType;
            return baseType != null && IsAssignableToGenericType(baseType, genericType);
        }
    }
}