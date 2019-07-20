using Ether.Network.Packets;
using NLog;
using Rhisis.Network.ISC.Packets;
using Rhisis.Network.ISC.Structures;
using Rhisis.Network;
using System;

namespace Rhisis.Cluster.ISC
{
    public static class ISCHandler
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        [PacketHandler(ISCPacketType.WELCOME)]
        public static void OnWelcome(ISCClient client, INetPacketStream packet)
        {
            ISCPacketFactory.SendAuthentication(client, 
                client.ClusterConfiguration.Id, 
                client.ClusterConfiguration.Host, 
                client.ClusterConfiguration.Name,
                client.ClusterConfiguration.Port);
        }

        [PacketHandler(ISCPacketType.AUTHENT_RESULT)]
        public static void OnAuthenticationResult(ISCClient client, INetPacketStream packet)
        {
            var authenticationResult = (ISCPacketCode)(packet.Read<uint>());

            switch (authenticationResult)
            {
                case ISCPacketCode.AUTH_SUCCESS:
                    Logger.Info("ISC client authenticated succesfully.");
                    return;
                case ISCPacketCode.AUTH_FAILED_CLUSTER_EXISTS:
                    Logger.Fatal("Unable to authenticate ISC client. Reason: an other Cluster server (with the same id) is already connected.");
                    break;
                case ISCPacketCode.AUTH_FAILED_UNKNOWN_SERVER:
                    Logger.Fatal("Unable to authenticate ISC client. Reason: ISC server doesn't recognize this server. You probably have to update all servers.");
                    break;
                default:
                    Logger.Trace("ISC authentification result: {0}", authenticationResult);
                    Logger.Fatal("Unable to authenticate ISC client. Reason: Cannot recognize ISC server. You probably have to update all servers.");
                    break;
            }

            //TODO: implement a peacefully shutdown.
            Console.ReadLine();
            Environment.Exit((int)authenticationResult);
        }

        [PacketHandler(ISCPacketType.UPDATE_CLUSTER_WORLDS_LIST)]
        public static void OnUpdateClusterWorldsList(ISCClient client, INetPacketStream packet)
        {
            var worldsCount = packet.Read<int>();

            client.WorldServers.Clear();

            for (int i = 0; i < worldsCount; i++)
            {
                var worldId = packet.Read<int>();
                var worldHost = packet.Read<string>();
                var worldName = packet.Read<string>();
                var worldParentClusterId = packet.Read<int>();

                if (worldParentClusterId != client.ClusterConfiguration.Id)
                    continue;

                client.WorldServers.Add(new WorldServerInfo(worldId, worldHost, worldName, worldParentClusterId));
            }
        }
    }
}
