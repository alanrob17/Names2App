using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Names.Repositories;
using Names.Services;
using Names.Services.Output;
using Serilog;

namespace Names
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Configure Serilog early to capture startup logs
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Async(a => a.Console())
                .CreateLogger();

            try
            {
                Log.Information("Starting Names...");

                var host = Host.CreateDefaultBuilder(args)
                    .ConfigureServices((context, services) =>
                    {
                        // Register services here
                        services.AddTransient<IArgumentRepository, ArgumentRepository>();
                        services.AddTransient<IArgumentService, ArgumentService>();
                        services.AddTransient<IFileService, FileService>();

                        services.AddTransient<IOutputService, OutputService>();

                        // Register ArgumentService with args
                        services.AddTransient<IArgumentService>(provider =>
                            new ArgumentService(
                                provider.GetRequiredService<IArgumentRepository>(),
                                provider.GetRequiredService<IOutputService>(),
                                provider.GetRequiredService<IFileService>(),
                                args
                            )
                        );

                        services.AddTransient<IFileRepository, FileRepository>();
                        services.AddTransient<IFileService, FileService>();
                    })
                    .UseSerilog()
                    .Build();

                using (var scope = host.Services.CreateScope())
                {
                    var argumentService = scope.ServiceProvider.GetRequiredService<IArgumentService>();
                    await argumentService.RunArgumentOperations();
                }

                Log.Information("Finished.");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
