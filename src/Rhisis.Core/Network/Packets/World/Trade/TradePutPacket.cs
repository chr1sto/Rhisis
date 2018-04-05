using Ether.Network.Packets;

namespace Rhisis.Core.Network.Packets.World.Trade
{
    public class TradePutPacket
    {
        public byte Position { get; private set; }

        public byte ItemType { get; private set; }

        public byte ItemId { get; private set; }

        public short Count { get; private set; }

        public TradePutPacket(INetPacketStream packet)
        {
            Position = packet.Read<byte>();
            ItemType = packet.Read<byte>();
            ItemId = packet.Read<byte>();
            Count = packet.Read<short>();
        }
    }
}
