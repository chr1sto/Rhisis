using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class BuyChipItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the tab.
        /// </summary>
        public byte Tab { get; private set; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public byte Id { get; private set; }

        /// <summary>
        /// Gets the quantity.
        /// </summary>
        public short Quantity { get; private set; }

        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Tab = packet.Read<byte>();
            Id = packet.Read<byte>();
            Quantity = packet.Read<short>();
            ItemId = packet.Read<uint>();
        }
    }
}