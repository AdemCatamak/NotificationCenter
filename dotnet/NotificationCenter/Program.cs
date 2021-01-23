using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationCenter.DataAccessLayer.DatabaseContext.ConfigModels;
using NotificationCenter.DataAccessLayer.DatabaseContext.MongoDbSection;

namespace NotificationCenter
{
    public class Program
    {
        private const string STARTUP_PROJECT_NAME = "NotificationCenter";

        public static void Main(string[] args)
        {
            var logger = LoggerFactory.Create(builder => builder.AddConsole())
                                      .CreateLogger<Program>();

            try
            {
                logger.LogInformation($"{STARTUP_PROJECT_NAME} Host is being created");

                using (var host = BuildHost(args))
                {
                    logger.LogInformation($"{STARTUP_PROJECT_NAME}'s preconditions are executing");

                    PrepareDb(host.Services).GetAwaiter().GetResult();

                    logger.LogInformation($"{STARTUP_PROJECT_NAME}'s preconditions are executed");

                    logger.LogInformation($"{STARTUP_PROJECT_NAME} is starting");
                    host.Run();
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
            }
            finally
            {
                logger.LogInformation($"{STARTUP_PROJECT_NAME} is shutting down");
            }
        }

        private static async Task PrepareDb(IServiceProvider serviceProvider)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                IServiceProvider provider = scope.ServiceProvider;
                var noSqlDbOption = provider.GetRequiredService<NoSqlDbOption>();
                switch (noSqlDbOption.NoSqlDbType)
                {
                    case NoSqlDbTypes.MongoDb:
                        IEnumerable<IMongoDbScript>? mongoDbScripts = provider.GetServices<IMongoDbScript>();
                        foreach (IMongoDbScript mongoDbScript in mongoDbScripts ?? new List<IMongoDbScript>())
                        {
                            await mongoDbScript.RunAsync(CancellationToken.None);
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static IHost BuildHost(string[] args)
        {
            var webHost = GetHostBuilder(args).Build();

            return webHost;
        }

        private static IHostBuilder GetHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                                  .ConfigureWebHostDefaults(webBuilder =>
                                                            {
                                                                webBuilder.UseStartup<Startup>()
                                                                          .ConfigureAppConfiguration((hostingContext, config) =>
                                                                                                     {
                                                                                                         config.AddJsonFile("appsettings.json");
                                                                                                         if (hostingContext.HostingEnvironment.IsDevelopment())
                                                                                                             config.AddJsonFile("appsettings.dev.json");
                                                                                                     })
                                                                          .ConfigureLogging((host, logging) =>
                                                                                            {
                                                                                                logging.SetMinimumLevel(LogLevel.Information);
                                                                                                logging.ClearProviders();
                                                                                                logging.AddConsole();
                                                                                            })
                                                                    ;
                                                            })
                                  .ConfigureServices((hostContext, services) => { });

            return hostBuilder;
        }
    }
}