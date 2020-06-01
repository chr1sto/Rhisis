using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Bank
{
    public class PutItemBankPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the slot.
        /// </summary>
        public byte Slot { get; private set; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public byte Id { get; private set; }

        /// <summary>
        /// Gets the item number.
        /// </summary>
        public short ItemNumber { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Slot = packet.Read<byte>();
            Id = packet.Read<byte>();
            ItemNumber = packet.Read<short>();
        }
    }
}