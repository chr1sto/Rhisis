using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="SfxIdPacket"/> structure.
    /// </summary>
    public class SfxIdPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the target id.
        /// </summary>
        public uint Target { get; set; }

        /// <summary>
        /// Gets the id of the hit SFX.
        /// </summary>
        public int IdSfxHit { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public uint Type { get; set; }

        /// <summary>
        /// Gets the skill.
        /// </summary>
        public uint Skill { get; set; }

        /// <summary>
        /// Gets the max damage count.
        /// </summary>
        public int MaxDamageCount { get; set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            this.Target = packet.Read<uint>();
            this.IdSfxHit = packet.Read<int>();
            this.Type = packet.Read<uint>();
            this.Skill = packet.Read<uint>();
            this.MaxDamageCount = packet.Read<int>();
        }
    }
}