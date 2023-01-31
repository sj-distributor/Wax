using Autofac.Extensions.DependencyInjection;
using Serilog;

namespace Wax.Api;

public partial class Program
{
    public static void Main(string[] args)
    {
        var configuration = GetConfiguration();

        Log.Logger = CreateSerilogLogger(configuration);

        try
        {
            Log.Information("Configuring api host ({ApplicationContext})... ", AppName);
            
            var webHost = CreateWebHostBuilder(args).Build();
            
            Log.Information("Starting api host ({ApplicationContext})...", AppName);

            webHost.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Program terminated unexpectedly!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IHostBuilder CreateWebHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureLogging(l => l.AddSerilog(Log.Logger))
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(builder => { builder.UseStartup<Startup>(); }).UseSerilog();

    private static IConfiguration GetConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables();

        return builder.Build();
    }

    private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
    {
        var seqServerUrl = configuration["Serilog:Seq:ServerUrl"];
        var seqApiKey = configuration["Serilog:Seq:ApiKey"];

        return new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.WithProperty("ApplicationContext", AppName)
            .Enrich.FromLogContext()
            .Enrich.WithCorrelationId()
            .WriteTo.Console()
            .WriteTo.Seq(seqServerUrl, apiKey: seqApiKey)
            .CreateLogger();
    }
}

public partial class Program
{
    private static readonly string Namespace = typeof(Startup).Namespace;

    private static readonly string AppName =
        Namespace[(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1)..];
}