using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class ExpBoxInfoPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; private set; }

        /// <summary>
        /// Gets the object id.
        /// </summary>
        public uint ObjectId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            PlayerId = packet.Read<uint>();
            ObjectId = packet.Read<uint>();
        }
    }
}