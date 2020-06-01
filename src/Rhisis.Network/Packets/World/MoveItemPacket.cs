using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class MoveItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the Item type.
        /// </summary>
        public byte ItemType { get; private set; }

        /// <summary>
        /// Gets the Item source slot.
        /// </summary>
        public byte SourceSlot { get; private set; }

        /// <summary>
        /// Gets the item destination slot.
        /// </summary>
        public byte DestinationSlot { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            ItemType = packet.Read<byte>();
            SourceSlot = packet.Read<byte>();
            DestinationSlot = packet.Read<byte>();
        }
    }
}
