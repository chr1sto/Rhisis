using Microsoft.Extensions.Logging;
using Rhisis.Cluster.CoreClient;
using Rhisis.Cluster.CoreClient.Packets;
using Rhisis.Cluster.WorldCluster.Packets;
using Rhisis.Cluster.WorldCluster.Server;
using Rhisis.Core.Resources;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Network.Core;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;

namespace Rhisis.Cluster.WorldCluster.Handlers
{
    [Handler]
    internal sealed class WorldHandler
    {
        private readonly ILogger<WorldHandler> _logger;
        private readonly IWorldClusterServer _worldClusterServer;
        private readonly IWorldPacketFactory _worldPacketFactory;
        
        private readonly ClusterConfiguration _clusterConfiguration;

        private readonly ICorePacketFactory _corePacketFactory;
        private readonly IClusterCoreClient _clusterCoreClient;
        private readonly ICache<int, WorldServerInfo> _worldCache;

        /// <summary>
        /// Creates a new <see cref="WorldHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="worldClusterServer">Core server instance.</param>
        /// <param name="clusterServer">Used to get the cluster configuration</param>
        /// <param name="clusterCoreClient">Core client</param>
        /// <param name="corePacketFactory">Packet creator for cluster/core communication</param>
        /// <param name="worldPacketFactory">Core server packet factory.</param>
        /// <param name="worldCache">The cache that holds the world information</param>
        public WorldHandler(ILogger<WorldHandler> logger, IWorldClusterServer worldClusterServer, IClusterServer clusterServer,
            IClusterCoreClient clusterCoreClient, ICorePacketFactory corePacketFactory,
            IWorldPacketFactory worldPacketFactory, ICache<int, WorldServerInfo> worldCache)
        {
            _logger = logger;
            _worldClusterServer = worldClusterServer;
            _clusterCoreClient = clusterCoreClient;
            _corePacketFactory = corePacketFactory;
            _worldPacketFactory = worldPacketFactory;
            _clusterConfiguration = clusterServer.ClusterConfiguration;
            _worldCache = worldCache;
        }
        
        [HandlerAction(CorePacketType.Authenticate)]
        public void OnAuthenticate(IWorldClusterServerClient client, INetPacketStream packet)
        {
            var id = packet.Read<int>();
            var name = packet.Read<string>();
            var host = packet.Read<string>();
            var port = packet.Read<int>();
            var parentClusterId = packet.Read<int>();
            var password = packet.Read<string>();

            if (parentClusterId != _clusterConfiguration.Id)
            {
                _logger.LogWarning($"World server '{name}' incoming connection from {client.Socket.RemoteEndPoint} refused. Reason: " +
                                   $"This seems as the wrong cluster server, wants cluster id: {parentClusterId}, " +
                                   $"this cluster id: {_clusterConfiguration.Id}.");
                
                _worldPacketFactory.SendAuthenticationResult(client, CoreAuthenticationResultType.FailedUnknownServer);
                _worldClusterServer.DisconnectClient(client.Id);
            }

            if (_worldClusterServer.WorldClusterConfiguration.Password != password)
            {
                _logger.LogWarning($"World server '{name}' incoming connection from {client.Socket.RemoteEndPoint} refused. Reason: " +
                                   $"wrong password.");
                
                _worldPacketFactory.SendAuthenticationResult(client, CoreAuthenticationResultType.FailedWrongPassword);
                _worldClusterServer.DisconnectClient(client.Id);
            }

            if (_worldCache.TryGetOrDefault(id) != null)
            {
                _logger.LogWarning($"World server '{name}' incoming connection from {client.Socket.RemoteEndPoint} refused." +
                                   $" Reason: An other World server with id '{id}' is already connected to Cluster Server '{_clusterConfiguration.Name}'.");
                _worldPacketFactory.SendAuthenticationResult(client, CoreAuthenticationResultType.FailedWorldExists);
                _worldClusterServer.DisconnectClient(client.Id);
                return;
            }
            
            client.ServerInfo = new WorldServerInfo(id, name, host, port, parentClusterId);
            _worldCache.Add(id, client.ServerInfo);

            _logger.LogInformation(
                $"World server '{name}' authenticated and is connected" +
                $" to world cluster server from {client.Socket.RemoteEndPoint}.");
                
            _worldPacketFactory.SendAuthenticationResult(client, CoreAuthenticationResultType.Success);
            _corePacketFactory.SendUpdateWorldList(_clusterCoreClient, _worldCache.Items);
        }
    }
}