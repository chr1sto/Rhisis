using Ether.Network.Packets;
using Rhisis.Cluster.Packets;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.Cluster;

namespace Rhisis.Cluster
{
    public static class ClusterHandler
    {
        [PacketHandler(PacketType.PING)]
        public static void OnPing(ClusterClient client, NetPacketBase packet)
        {
            var pingPacket = new PingPacket(packet);

            ClusterPacketFactory.SendPong(client, pingPacket.Time);
        }

        [PacketHandler(PacketType.GETPLAYERLIST)]
        public static void OnGetPlayerList(ClusterClient client, NetPacketBase packet)
        {
            var getPlayerListPacket = new GetPlayerListPacket(packet);
            Logger.Info("GetPlayerList()");

            ClusterPacketFactory.SendPlayerList(client, getPlayerListPacket.AuthenticationKey);
        }
    }
}
