using Ether.Network.Packets;

namespace Rhisis.Core.Network.Packets.World.Trade
{
    public class TradePutPacket
    {
        public readonly byte Position;

        public readonly byte ItemType;

        public readonly byte ItemId;

        public readonly short Count;

        public TradePutPacket(INetPacketStream packet)
        {
            Position = packet.Read<byte>();
            ItemType = packet.Read<byte>();
            ItemId = packet.Read<byte>();
            Count = packet.Read<short>();
        }
    }
}
