using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Trade
{
    public class TradePutPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the target position slot in the trade container.
        /// </summary>
        public byte Position { get; private set; }

        /// <summary>
        /// Gets the item type.
        /// </summary>
        public byte ItemType { get; private set; }

        /// <summary>
        /// Gets the item unique id.
        /// </summary>
        public byte ItemUniqueId { get; private set; }

        /// <summary>
        /// Gets the amount of items to put into the trade.
        /// </summary>
        public short Count { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Position = packet.Read<byte>();
            ItemType = packet.Read<byte>();
            ItemUniqueId = packet.Read<byte>();
            Count = packet.Read<short>();
        }
    }
}
