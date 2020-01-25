using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class BuyItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the tab.
        /// </summary>
        public byte Tab { get; private set; }

        /// <summary>
        /// Gets the slot of the item.
        /// </summary>
        public byte Slot { get; private set; }

        /// <summary>
        /// Gets the quantity of items.
        /// </summary>
        public short Quantity { get; private set; }

        /// <summary>
        /// Gets the Item Id.
        /// </summary>
        public int ItemId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Tab = packet.Read<byte>();
            Slot = packet.Read<byte>();
            Quantity = packet.Read<short>();
            ItemId = packet.Read<int>();
        }
    }
}
