using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ExperiencePacket"/> structure.
    /// </summary>
    public class ExperiencePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the experience amount.
        /// </summary>
        public long Experience { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            this.Experience = packet.Read<long>();
        }

        /// <summary>
        /// Compares two <see cref="ExperiencePacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ExperiencePacket"/></param>
        public bool Equals(ExperiencePacket other)
        {
            return this.Experience == other.Experience;
        }
    }
}