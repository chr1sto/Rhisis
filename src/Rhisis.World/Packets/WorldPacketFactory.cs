using Ether.Network;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Packets
{
    public static class WorldPacketFactory
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
    }
}
