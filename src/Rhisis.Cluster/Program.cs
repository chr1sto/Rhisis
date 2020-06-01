using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rhisis.Cluster.CoreClient;
using Rhisis.Cluster.CoreClient.Packets;
using Rhisis.Cluster.Packets;
using Rhisis.Core.Extensions;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Network.Packets;
using Sylver.HandlerInvoker;
using Sylver.Network.Data;
using System.IO;
using System.Threading.Tasks;
using Rhisis.Cluster.WorldCluster;
using Rhisis.Cluster.WorldCluster.Packets;
using Rhisis.Cluster.WorldCluster.Server;
using Rhisis.Network.Core;

namespace Rhisis.Cluster
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
                    configApp.AddJsonFile(ConfigurationConstants.ClusterServerPath, optional: false);
                    configApp.AddJsonFile(ConfigurationConstants.DatabasePath, optional: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.AddMemoryCache();

                    services.Configure<ClusterConfiguration>(hostContext.Configuration.GetSection(ConfigurationConstants.ClusterServer));
                    services.Configure<WorldClusterConfiguration>(hostContext.Configuration.GetSection(ConfigurationConstants.WorldClusterServer));
                    services.Configure<CoreConfiguration>(hostContext.Configuration.GetSection(ConfigurationConstants.CoreServer));

                    services.AddDatabase(hostContext.Configuration);
                    services.AddHandlers();
                    services.AddGameResources();
                    
                    // World cluster server configuration
                    services.AddSingleton<IWorldClusterServer, WorldClusterServer>();
                    services.AddSingleton<IWorldPacketFactory, WorldPacketFactory>();
                    services.AddSingleton<IHostedService, WorldClusterServerService>();
                    services.AddSingleton<ICache<int, WorldServerInfo>, WorldCache>();

                    // Cluster server configuration
                    services.AddSingleton<IClusterServer, ClusterServer>();
                    services.AddSingleton<IClusterPacketFactory, ClusterPacketFactory>();
                    services.AddSingleton<IHostedService, ClusterServerService>();

                    // Core client configuration
                    services.AddSingleton<IClusterCoreClient, ClusterCoreClient>();
                    services.AddSingleton<ICorePacketFactory, CorePacketFactory>();
                    services.AddSingleton<IHostedService, ClusterCoreClientService>();
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
                .SetConsoleCulture(culture)
                .UseConsoleLifetime()
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