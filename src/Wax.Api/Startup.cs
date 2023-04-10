using Autofac;
using Wax.Api.Authentication;
using Wax.Api.Extensions;
using Wax.Core.Services.Identity;
using Serilog;
using Wax.Api.Filters;
using Wax.Core;

namespace Wax.Api;

public class Startup
{
    private IConfiguration Configuration { get; }

    private IServiceCollection _serviceCollection;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // ConfigureServices is where you register dependencies. This gets
    // called by the runtime before the ConfigureContainer method, below.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(options => { options.Filters.Add<GlobalExceptionFilter>(); });
        services.AddOptions();
        services.AddCustomSwagger();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddLogging();
        services.AddHttpContextAccessor();

        _serviceCollection = services;
    }

    // ConfigureContainer is where you can register things directly
    // with Autofac. This runs after ConfigureServices so the things
    // here will override registrations made in ConfigureServices.
    // Don't build the container; that gets done for you by the factory.
    public void ConfigureContainer(ContainerBuilder builder)
    {
        var serviceProvider = _serviceCollection.BuildServiceProvider();

        var currentUser = serviceProvider.GetRequiredService<ICurrentUser>();

        // Register your own things directly with Autofac here. Don't
        // call builder.Populate(), that happens in AutofacServiceProviderFactory
        // for you.
        builder.RegisterModule(new ApplicationModule(Log.Logger, currentUser, Configuration,
            typeof(Startup).Assembly));
    }

    // Configure is where you add middleware. This is called after
    // ConfigureContainer. You can use IApplicationBuilder.ApplicationServices
    // here if you need to resolve things from the container.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Barcode.Api.xml"); });
        }

        app.UseRouting();
        app.UseCors();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}