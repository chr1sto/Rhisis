using System;
using System.Collections.Generic;
using System.Text;
using Ether.Network.Packets;

namespace Rhisis.Core.Network.Packets.World.Trade
{
    public class TradeRequestPacket
    {
        public int Target { get; }

        public TradeRequestPacket(INetPacketStream packet)
        {
            this.Target = packet.Read<int>();
        }
    }
}
