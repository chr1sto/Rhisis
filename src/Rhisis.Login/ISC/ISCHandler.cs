using Ether.Network.Packets;
using NLog;
using Rhisis.Core.ISC;
using Rhisis.Core.ISC.Packets;
using Rhisis.Core.ISC.Structures;
using Rhisis.Core.Network;
using Rhisis.Login.ISC.Packets;

namespace Rhisis.Login.ISC
{
    public static class ISCHandler
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        [PacketHandler(ISCPacketType.AUTHENT)]
        public static void OnAuthenticate(ISCClient client, INetPacketStream packet)
        {
            var id = packet.Read<int>();
            var host = packet.Read<string>();
            var name = packet.Read<string>();
            var type = packet.Read<byte>();

            client.Type = (ISCServerType)type;

            if (client.Type == ISCServerType.Cluster)
            {
                if (client.IcsServer.HasClusterWithId(id))
                {
                    Logger.Warn("Cluster Server '{0}' incoming connection from {1} refused. Reason: An other Cluster server with id '{2}' is already connected.", name, client.RemoteEndPoint, id);
                    ISCPacketFactory.SendAuthenticationResult(client, ISCPacketCode.AUTH_FAILED_CLUSTER_EXISTS);
                    client.IcsServer.DisconnectClient(client.Id);

                    return;
                }

                client.ServerInfo = new ClusterServerInfo(id, host, name);
                
                ISCPacketFactory.SendAuthenticationResult(client, ISCPacketCode.AUTH_SUCCESS);
                Logger.Info("Cluster server '{0}' connected to ISC server from {1}.", name, client.RemoteEndPoint);
            }
            else if (client.Type == ISCServerType.World)
            {
                var clusterId = packet.Read<int>();
                // TODO: read more informations about world server if needed

                if (!client.IcsServer.HasClusterWithId(clusterId))
                {
                    // No cluster for this server
                    Logger.Warn("World server '{0}' incoming connection from {1} refused. Reason: Cluster server with id '{2}' is not connected.", name, client.RemoteEndPoint, clusterId);
                    ISCPacketFactory.SendAuthenticationResult(client, ISCPacketCode.AUTH_FAILED_NO_CLUSTER);
                    client.IcsServer.DisconnectClient(client.Id);
                    
                    return;
                }

                ISCClient parentCluster = client.IcsServer.GetCluster(clusterId);
                var clusterInfo = parentCluster.GetServerInfo<ClusterServerInfo>();

                if (client.IcsServer.HasWorldInCluster(clusterId, id))
                {
                    // World already exists in cluster
                    Logger.Warn("World server '{0}' incoming connection from {1} refused. Reason: An other World server with id '{2}' is already connected to Cluster Server '{3}'.", name, client.RemoteEndPoint, id, clusterInfo.Name);
                    ISCPacketFactory.SendAuthenticationResult(client, ISCPacketCode.AUTH_FAILED_WORLD_EXISTS);
                    client.IcsServer.DisconnectClient(client.Id);
                    
                    return;
                }

                client.ServerInfo = new WorldServerInfo(id, host, name, clusterId);
                clusterInfo.WorldServers.Add(client.ServerInfo as WorldServerInfo);
                ISCPacketFactory.SendAuthenticationResult(client, ISCPacketCode.AUTH_SUCCESS);
                ISCPacketFactory.SendUpdateWorldList(parentCluster, clusterInfo.WorldServers);
                Logger.Info("World server '{0}' join cluster '{1}' and is connected to ISC server from {2}.", 
                    name, clusterInfo.Name, client.RemoteEndPoint);
            }
            else
            {
                Logger.Warn("Incoming ISC connection from {0} refused. Reason: server type is unknown.", client.RemoteEndPoint);
                ISCPacketFactory.SendAuthenticationResult(client, ISCPacketCode.AUTH_FAILED_UNKNOWN_SERVER);
                client.IcsServer.DisconnectClient(client.Id);
            }
        }
    }
}
