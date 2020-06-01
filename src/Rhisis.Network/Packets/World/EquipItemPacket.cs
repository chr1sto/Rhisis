using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class EquipItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the item unique id.
        /// </summary>
        public int UniqueId { get; private set; }

        /// <summary>
        /// Gets the equip item destination part.
        /// </summary>
        public int Part { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            UniqueId = packet.Read<int>();
            Part = packet.Read<int>();
        }
    }
}
