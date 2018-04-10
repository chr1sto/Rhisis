using Ether.Network.Packets;

namespace Rhisis.Core.Network.Packets.World.Trade
{
    public class TradePutGoldPacket
    {
        public int Gold { get; private set; }

        public TradePutGoldPacket(INetPacketStream packet)
        {
            Gold = packet.Read<int>();
        }
    }
}
