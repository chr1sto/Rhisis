using Microsoft.Extensions.Logging;
using Rhisis.Login.Core.Packets;
using Rhisis.Network.Core;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;

namespace Rhisis.Login.Core.Handlers
{
    [Handler]
    internal sealed class ClusterHandler
    {
        private readonly ILogger<ClusterHandler> _logger;
        private readonly ICoreServer _coreServer;
        private readonly ICorePacketFactory _corePacketFactory;

        /// <summary>
        /// Creates a new <see cref="ClusterHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="coreServer">Core server instance.</param>
        /// <param name="corePacketFactory">Core server packet factory.</param>
        public ClusterHandler(ILogger<ClusterHandler> logger, ICoreServer coreServer, 
            ICorePacketFactory corePacketFactory)
        {
            _logger = logger;
            _coreServer = coreServer;
            _corePacketFactory = corePacketFactory;
        }

        /// <summary>
        /// Authenticates a core server client.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="packet">Incoming packet stream.</param>
        [HandlerAction(CorePacketType.Authenticate)]
        public void OnAuthenticate(CoreServerClient client, INetPacketStream packet)
        {
            var id = packet.Read<int>();
            var name = packet.Read<string>();
            var host = packet.Read<string>();
            var port = packet.Read<int>();
            var password = packet.Read<string>();

            if (_coreServer.CoreConfiguration.Password != password)
            {
                _logger.LogWarning($"Cluster server '{name}' incoming connection from {client.Socket.RemoteEndPoint} refused. Reason: " +
                                   $"wrong password.");
                
                _corePacketFactory.SendAuthenticationResult(client, CoreAuthenticationResultType.FailedWrongPassword);
                _coreServer.DisconnectClient(client.Id);
            }

            if (_coreServer.HasCluster(id))
            {
                _logger.LogWarning($"Cluster Server '{name}' incoming connection from {client.Socket.RemoteEndPoint} refused." +
                                   $" Reason: An other Cluster server with id '{id}' is already connected.");
                _corePacketFactory.SendAuthenticationResult(client, CoreAuthenticationResultType.FailedClusterExists);
                _coreServer.DisconnectClient(client.Id);
                return;
            }

            client.ServerInfo = new ClusterServerInfo(id, name, host, port);
            _logger.LogInformation($"Cluster server '{name}' connected to core server from {client.Socket.RemoteEndPoint}.");

            _corePacketFactory.SendAuthenticationResult(client, CoreAuthenticationResultType.Success);
        }
        
        /// <summary>
        /// Updates the cluster's world servers list.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [HandlerAction(CorePacketType.UpdateClusterWorldsList)]
        public void OnUpdateClusterWorldsList(CoreServerClient client, INetPacketStream packet)
        {
            int numberOfWorldServers = packet.Read<int>();

            var clusterServerInfo = client.ServerInfo as ClusterServerInfo;
            if (clusterServerInfo == null)
            {
                _logger.LogCritical("Could not update the cluster world list as the server information does not seem to be of the right type");
                return;
            }
            
            clusterServerInfo.Worlds.Clear();
        
            for (var i = 0; i < numberOfWorldServers; ++i)
            {
                int serverId = packet.Read<int>();
                string serverHost = packet.Read<string>();
                string serverName = packet.Read<string>();
                int serverPort = packet.Read<int>();
                int parentClusterId = packet.Read<int>();

                var worldServer = new WorldServerInfo(serverId, serverName, serverHost, serverPort, parentClusterId);
                clusterServerInfo.Worlds.Add(worldServer);
            }
            
            _logger.LogInformation($"[{clusterServerInfo.Id}] Cluster '{clusterServerInfo.Name}' has {clusterServerInfo.Worlds.Count} worlds");
            clusterServerInfo.Worlds.ForEach(w =>
            {
                _logger.LogInformation($"   ({w.Id}) World '{w.Name}' @ {w.Host}:{w.Port}");
            });
        }
    }
}
