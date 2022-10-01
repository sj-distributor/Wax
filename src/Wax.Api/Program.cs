using Autofac.Extensions.DependencyInjection;
using Serilog;

namespace Wax.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var application = typeof(Program).Namespace ?? "Wax.Api";
        
        try
        {
            var webHost = CreateWebHostBuilder(args).Build();

            webHost.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", application);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IHostBuilder CreateWebHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(builder => { builder.UseStartup<Startup>(); });
}