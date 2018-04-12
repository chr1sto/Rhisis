using Ether.Network.Packets;

namespace Rhisis.Core.Network.Packets.World.Trade
{
    public class TradePutGoldPacket
    {
        public readonly int Gold;

        public TradePutGoldPacket(INetPacketStream packet)
        {
            Gold = packet.Read<int>();
        }
    }
}
