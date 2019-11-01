using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ScriptAddExperiencePacket"/> structure.
    /// </summary>
    public struct ScriptAddExperiencePacket : IEquatable<ScriptAddExperiencePacket>
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public long Experience { get; set; }

        /// <summary>
        /// Creates a new <see cref="ScriptAddExperiencePacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ScriptAddExperiencePacket(INetPacketStream packet)
        {
            this.Experience = packet.Read<long>();
        }

        /// <summary>
        /// Compares two <see cref="ScriptAddExperiencePacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ScriptAddExperiencePacket"/></param>
        public bool Equals(ScriptAddExperiencePacket other)
        {
            return this.Experience == other.Experience;
        }
    }
}