using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ExperiencePacket"/> structure.
    /// </summary>
    public struct ExperiencePacket : IEquatable<ExperiencePacket>
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public long Experience { get; set; }

        /// <summary>
        /// Creates a new <see cref="ExperiencePacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ExperiencePacket(INetPacketStream packet)
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