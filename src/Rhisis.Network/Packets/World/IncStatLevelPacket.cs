using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class IncStatLevelPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public byte Id { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Id = packet.Read<byte>();
        }
    }
}