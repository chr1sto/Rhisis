using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class SellItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the item's unique id.
        /// </summary>
        public byte ItemUniqueId { get; set; }

        /// <summary>
        /// Gets the item quantity to sell.
        /// </summary>
        public short Quantity { get; set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            ItemUniqueId = packet.Read<byte>();
            Quantity = packet.Read<short>();
        }
    }
}
