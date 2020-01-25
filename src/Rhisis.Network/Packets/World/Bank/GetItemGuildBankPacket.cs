using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Bank
{
    public class GetItemGuildBankPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public byte Id { get; private set; }

        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; private set; }

        /// <summary>
        /// Gets the mode.
        /// </summary>
        public byte Mode { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Id = packet.Read<byte>();
            ItemId = packet.Read<uint>();
            Mode = packet.Read<byte>();
        }
    }
}