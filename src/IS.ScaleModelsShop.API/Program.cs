using System.Diagnostics.CodeAnalysis;

namespace IS.ScaleModelsShop.API;

/// <summary>
///     Application main class.
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Excluded as it is the entry point to application.")]
public class Program
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Program" /> class.
    /// </summary>
    protected Program()
    {
    }

    /// <summary>
    ///     Application entry point, creates and runs application instance.
    /// </summary>
    /// <param name="args">Build arguments array.</param>
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment.EnvironmentName;
                var jsonFileName = $"appsettings.{env}.json";

                config.SetBasePath(context.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile(jsonFileName, true, true)
                    .AddEnvironmentVariables()
                    .Build();
            })
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}