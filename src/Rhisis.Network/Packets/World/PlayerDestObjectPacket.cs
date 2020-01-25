using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="PlayerDestObjectPacket"/> structure.
    /// </summary>
    public class PlayerDestObjectPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the target object id.
        /// </summary>
        public uint TargetObjectId { get; private set; }

        /// <summary>
        /// Gets the distance to the target.
        /// </summary>
        public float Distance { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            TargetObjectId = packet.Read<uint>();
            Distance = packet.Read<float>();
        }
    }
}