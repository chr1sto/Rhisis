using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rhisis.Core.Handlers;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using System.IO;
using System.Threading.Tasks;

namespace Rhisis.Login
{
    public static class Program
    {
        private static async Task Main()
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                    configApp.AddJsonFile("config/login.json", optional: false);
                    configApp.AddJsonFile("config/database.json", optional: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<LoginConfiguration>(hostContext.Configuration.GetSection("loginServer"));
                    services.Configure<ISCConfiguration>(hostContext.Configuration.GetSection("isc"));
                    services.RegisterDatabaseServices(hostContext.Configuration.Get<DatabaseConfiguration>());

                    services.AddHandlers();
                    services.AddSingleton<ILoginServer, LoginServer>();
                    services.AddSingleton<IHostedService, LoginServerService>(); // LoginServer service starting the server
                })
                .ConfigureLogging(builder =>
                {
                    builder.AddFilter("Microsoft", LogLevel.Warning);
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddNLog(new NLogProviderOptions
                    {
                        CaptureMessageTemplates = true,
                        CaptureMessageProperties = true
                    });
                })
                .Build();

            await host.RunAsync();
        }
    }
}