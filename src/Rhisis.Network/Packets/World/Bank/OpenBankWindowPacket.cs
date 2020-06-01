using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Bank
{
    public class OpenBankWindowPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public uint Id { get; private set; }

        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Id = packet.Read<uint>();
            ItemId = packet.Read<uint>();
        }
    }
}