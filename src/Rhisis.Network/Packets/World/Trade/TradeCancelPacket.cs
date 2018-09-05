using System;
using System.Collections.Generic;
using System.Text;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.Trade
{
    public class TradeCancelPacket
    {
        public int Mode { get; }

        public TradeCancelPacket(INetPacketStream packet)
        {
            Mode = packet.Read<int>();
        }
    }
}
