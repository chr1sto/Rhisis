using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class ScriptEquipItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; private set; }

        /// <summary>
        /// Gets the option.
        /// </summary>
        public int Option { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            ItemId = packet.Read<uint>();
            Option = packet.Read<int>();
        }
    }
}