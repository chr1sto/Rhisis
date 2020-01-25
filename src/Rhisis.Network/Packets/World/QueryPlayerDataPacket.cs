using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class QueryPlayerDataPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; private set; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        public int Version { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            PlayerId = packet.Read<uint>();
            Version = packet.Read<int>();
        }
    }
}
