using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Cluster.Models;
using Rhisis.Cluster.WorldCluster.Packets;
using Rhisis.Core.Structures.Configuration;
using Sylver.HandlerInvoker;
using Sylver.Network.Server;

namespace Rhisis.Cluster.WorldCluster.Server
{
    public class WorldClusterServer : NetServer<WorldClusterServerClient>, IWorldClusterServer
    {
        private const int ClientBacklog = 50;
        private const int ClientBufferSize = 64;
        
        private readonly ILogger<WorldClusterServer> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHandlerInvoker _handlerInvoker;
        private readonly IWorldCache _worldCache;

        public WorldClusterConfiguration WorldClusterConfiguration { get; }

        /// <summary>
        /// Creates a new <see cref="WorldClusterServer"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="configuration">World cluster server configuration.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="handlerInvoker">Packet invoker</param>
        /// <param name="worldCache">World server information cache</param>
        public WorldClusterServer(ILogger<WorldClusterServer> logger, IOptions<WorldClusterConfiguration> configuration, 
            IServiceProvider serviceProvider, IHandlerInvoker handlerInvoker, IWorldCache worldCache)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _handlerInvoker = handlerInvoker;
            _worldCache = worldCache;
            
            WorldClusterConfiguration = configuration.Value;
            ServerConfiguration = new NetServerConfiguration(WorldClusterConfiguration.Host, 
                WorldClusterConfiguration.Port, 
                ClientBacklog, 
                ClientBufferSize);
        }
        
        protected override void OnAfterStart()
        {
            _logger.LogInformation($"'{nameof(WorldClusterServer)}' is started and listening on {ServerConfiguration.Host}:{ServerConfiguration.Port}.");
        }

        /// <inheritdoc />
        protected override void OnClientConnected(WorldClusterServerClient client)
        {
            _logger.LogInformation($"New incoming world server client connection from {client.Socket.RemoteEndPoint}.");
            
            var packetFactory = _serviceProvider.GetRequiredService<IWorldPacketFactory>();
            client.Initialize(_serviceProvider.GetRequiredService<ILogger<WorldClusterServerClient>>(), _handlerInvoker);
            packetFactory.SendHandshake(client);
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(WorldClusterServerClient client)
        {
            _worldCache.Remove(client.ServerInfo);
            _logger.LogInformation($"World server {client.ServerInfo.Name} disconnected, removed {client.ServerInfo.Name} from the world cache");
        }
    }
}