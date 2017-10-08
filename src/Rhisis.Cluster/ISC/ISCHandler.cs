using Ether.Network.Packets;
using Rhisis.Core.IO;
using Rhisis.Core.ISC.Packets;
using Rhisis.Core.ISC.Structures;
using Rhisis.Core.Network;

namespace Rhisis.Cluster.ISC
{
    public static class ISCHandler
    {
        [PacketHandler(InterPacketType.Welcome)]
        public static void OnWelcome(ISCClient client, NetPacketBase packet)
        {
            ISCPackets.SendAuthentication(client, client.Configuration.Id, client.Configuration.Host, client.Configuration.Name);
        }

        [PacketHandler(InterPacketType.AuthenticationResult)]
        public static void OnAuthenticationResult(ISCClient client, NetPacketBase packet)
        {
            var authenticationResult = packet.Read<uint>();

            Logger.Debug("Authentication result: {0}", (InterServerError)authenticationResult);
        }

        [PacketHandler(InterPacketType.UpdateClusterWorldsList)]
        public static void OnUpdateClusterWorldsList(ISCClient client, NetPacketBase packet)
        {
            client.Worlds.Clear();
            var worldsCount = packet.Read<int>();

            for (int i = 0; i < worldsCount; i++)
            {
                var worldId = packet.Read<int>();
                var worldHost = packet.Read<string>();
                var worldName = packet.Read<string>();
                var worldParentClusterId = packet.Read<int>();

                if (worldParentClusterId != client.Configuration.Id)
                    continue;

                client.Worlds.Add(new WorldServerInfo(worldId, worldHost, worldName, worldParentClusterId));
            }
        }
    }
}
