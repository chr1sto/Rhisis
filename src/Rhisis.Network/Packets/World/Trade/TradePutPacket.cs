using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.Trade
{
    public class TradePutPacket
    {
        public byte Position { get; }

        public byte ItemType { get; }

        public byte ItemId { get; }

        public short Count { get; }

        public TradePutPacket(INetPacketStream packet)
        {
            Position = packet.Read<byte>();
            ItemType = packet.Read<byte>();
            ItemId = packet.Read<byte>();
            Count = packet.Read<short>();
        }
    }
}
