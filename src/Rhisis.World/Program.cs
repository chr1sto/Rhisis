using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Extensions;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Database;
using Rhisis.Network.Packets;
using Sylver.HandlerInvoker;
using Sylver.Network.Data;
using System.IO;
using System.Threading.Tasks;
using Rhisis.World.ClusterClient;

namespace Rhisis.World
{
    public static class Program
    {
        private static async Task Main()
        {
            const string culture = "en-US";

            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                    configApp.AddJsonFile(ConfigurationConstants.WorldServerPath, optional: false);
                    configApp.AddJsonFile(ConfigurationConstants.DatabasePath, optional: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.AddMemoryCache();
                    services.Configure<WorldConfiguration>(hostContext.Configuration.GetSection(ConfigurationConstants.WorldServer));
                    services.Configure<WorldClusterConfiguration>(hostContext.Configuration.GetSection(ConfigurationConstants.WorldClusterServer));

                    services.AddDatabase(hostContext.Configuration);
                    services.AddHandlers();
                    services.AddGameResources();
                    services.AddInjectableServices();

                    // World server configuration
                    services.AddSingleton<IWorldServer, WorldServer>();
                    services.AddSingleton<IHostedService, WorldServerService>();
                    
                    // World cluster server client configuration
                    services.AddSingleton<IWorldClusterClient, WorldClusterClient>();
                    services.AddSingleton<IHostedService, WorldClusterClientService>();
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
                .UseConsoleLifetime()
                .SetConsoleCulture(culture)
                .Build();

            await host
                .AddHandlerParameterTransformer<INetPacketStream, IPacketDeserializer>((source, dest) =>
                {
                    dest?.Deserialize(source);
                    return dest;
                })
                .RunAsync();
        }
    }
}