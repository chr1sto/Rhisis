using Microsoft.Extensions.Logging;
using Rhisis.Login.Core.Packets;
using Rhisis.Network.Core;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;

namespace Rhisis.Login.Core.Handlers
{
    [Handler]
    internal sealed class AuthenticationHandler
    {
        private readonly ILogger<AuthenticationHandler> _logger;
        private readonly ICoreServer _coreServer;
        private readonly ICorePacketFactory _corePacketFactory;

        /// <summary>
        /// Creates a new <see cref="AuthenticationHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="coreServer">Core server instance.</param>
        /// <param name="corePacketFactory">Core server packet factory.</param>
        public AuthenticationHandler(ILogger<AuthenticationHandler> logger, ICoreServer coreServer, ICorePacketFactory corePacketFactory)
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
            var type = (ServerType)packet.Read<byte>();

            if (type == ServerType.Cluster)
            {
                if (_coreServer.HasCluster(id))
                {
                    _logger.LogWarning($"Cluster Server '{name}' incoming connection from {client.Socket.RemoteEndPoint} refused. Reason: An other Cluster server with id '{id}' is already connected.");
                    _corePacketFactory.SendAuthenticationResult(client, CoreAuthenticationResultType.FailedClusterExists);
                    _coreServer.DisconnectClient(client.Id);
                    return;
                }

                client.ServerInfo = new ClusterServerInfo(id, name, host, port);
                _logger.LogInformation($"Cluster server '{name}' connected to ISC server from {client.Socket.RemoteEndPoint}.");
            }
            else if (type == ServerType.World)
            {
                var parentClusterId = packet.Read<int>();

                CoreServerClient parentClusterServer = _coreServer.GetClusterServer(parentClusterId);

                if (parentClusterServer == null)
                {
                    _logger.LogWarning($"World server '{name}' incoming connection from {client.Socket.RemoteEndPoint} refused. Reason: Cluster server with id '{parentClusterId}' is not connected.");

                    _corePacketFactory.SendAuthenticationResult(client, CoreAuthenticationResultType.FailedNoCluster);
                    _coreServer.DisconnectClient(client.Id);
                    return;
                }

                if (_coreServer.HasWorld(parentClusterId, id))
                {
                    _logger.LogWarning($"World server '{name}' incoming connection from {client.Socket.RemoteEndPoint} refused. Reason: An other World server with id '{id}' is already connected to Cluster Server '{parentClusterServer.ServerInfo.Name}'.");
                    _corePacketFactory.SendAuthenticationResult(client, CoreAuthenticationResultType.FailedWorldExists);
                    _coreServer.DisconnectClient(client.Id);
                    return;
                }

                var cluster = parentClusterServer.ServerInfo as ClusterServerInfo;
                if (cluster is null)
                {
                    _logger.LogWarning($"World server '{name}' incoming connection from {client.Socket.RemoteEndPoint} refused. Reason: Parent cluster server is not a cluster server.");
                    return;
                }

                var worldInfo = new WorldServerInfo(id, name, host, port, parentClusterId);

                cluster.Worlds.Add(worldInfo);
                client.ServerInfo = worldInfo;
                _corePacketFactory.SendUpdateWorldList(parentClusterServer, cluster.Worlds);
                _logger.LogInformation($"World server '{name}' join cluster '{cluster.Name}' and is connected to ISC server from {client.Socket.RemoteEndPoint}.");
            }
            else
            {
                _logger.LogWarning($"Incoming core connection from {client.Socket.RemoteEndPoint} refused. Reason: server type is unknown.");
                _corePacketFactory.SendAuthenticationResult(client, CoreAuthenticationResultType.FailedUnknownServer);
                _coreServer.DisconnectClient(client.Id);
                return;
            }

            _corePacketFactory.SendAuthenticationResult(client, CoreAuthenticationResultType.Success);
        }
    }
}
