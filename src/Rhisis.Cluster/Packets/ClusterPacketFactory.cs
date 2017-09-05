using Ether.Network;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Cluster.Packets
{
    public static class ClusterPacketFactory
    {
        public static void SendWelcome(NetConnection client, uint sessionId)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.WELCOME);
                packet.Write(sessionId);

                client.Send(packet);
            }
        }

        public static void SendPong(NetConnection client, int time)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.PING);
                packet.Write(time);

                client.Send(packet);
            }
        }

        public static void SendPlayerList(NetConnection client, int authenticationKey)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.PLAYER_LIST);
                packet.Write(authenticationKey);
                packet.Write(0); // player count

                client.Send(packet);
            }
        }
    }
}
