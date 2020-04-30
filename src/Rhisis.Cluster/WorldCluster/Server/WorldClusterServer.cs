using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Cluster.CoreClient;
using Rhisis.Cluster.CoreClient.Packets;
using Rhisis.Cluster.WorldCluster.Packets;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Network.Core;
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

        private readonly ICorePacketFactory _corePacketFactory;
        private readonly IClusterCoreClient _clusterCoreClient;

        public WorldClusterConfiguration WorldClusterConfiguration { get; }

        public IEnumerable<WorldServerInfo> Worlds => Clients
            .Where(c => c.ServerInfo != null)
            .Select(c => c.ServerInfo);

        /// <summary>
        /// Creates a new <see cref="WorldClusterServer"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="configuration">World cluster server configuration.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <param name="handlerInvoker">Packet invoker</param>
        /// <param name="clusterCoreClient">Core client</param>
        /// <param name="corePacketFactory">Packet creator for cluster/core communication</param>
        public WorldClusterServer(ILogger<WorldClusterServer> logger, IOptions<WorldClusterConfiguration> configuration, 
            IServiceProvider serviceProvider, IHandlerInvoker handlerInvoker,
            IClusterCoreClient clusterCoreClient, ICorePacketFactory corePacketFactory)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _handlerInvoker = handlerInvoker;

            _clusterCoreClient = clusterCoreClient;
            _corePacketFactory = corePacketFactory;
            
            WorldClusterConfiguration = configuration.Value;
            ServerConfiguration = new NetServerConfiguration(WorldClusterConfiguration.Host, 
                WorldClusterConfiguration.Port, 
                ClientBacklog, 
                ClientBufferSize);
        }

        public bool HasWorldWithId(int id)
        {
            return Worlds.SingleOrDefault(w => w.Id == id) != null;
        }

        protected override void OnAfterStart()
        {
            _logger.LogInformation($"'{nameof(WorldClusterServer)}' is started and listening on {ServerConfiguration.Host}:{ServerConfiguration.Port}.");
        }

        /// <inheritdoc />
        protected override void OnClientConnected(WorldClusterServerClient client)
        {
            var packetFactory = _serviceProvider.GetRequiredService<IWorldPacketFactory>();
            client.Initialize(_serviceProvider.GetRequiredService<ILogger<WorldClusterServerClient>>(), _handlerInvoker);
            packetFactory.SendHandshake(client);
            
            _logger.LogInformation($"New incoming world server client connection from {client.Socket.RemoteEndPoint}.");
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(WorldClusterServerClient client)
        {
            _corePacketFactory.SendUpdateWorldList(_clusterCoreClient, Worlds);
            
            _logger.LogInformation($"World server {client.ServerInfo.Name} disconnected, removed {client.ServerInfo.Name} from the world cache");
        }
    }
}