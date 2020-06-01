using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class RemoveItemLevelDownPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public uint Id { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Id = packet.Read<uint>();
        }
    }
}