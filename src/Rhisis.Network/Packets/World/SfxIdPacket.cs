using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="SfxIdPacket"/> structure.
    /// </summary>
    public struct SfxIdPacket : IEquatable<SfxIdPacket>
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

        /// <summary>
        /// Creates a new <see cref="SfxIdPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public SfxIdPacket(INetPacketStream packet)
        {
            this.Target = packet.Read<uint>();
            this.IdSfxHit = packet.Read<int>();
            this.Type = packet.Read<uint>();
            this.Skill = packet.Read<uint>();
            this.MaxDamageCount = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="SfxIdPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="SfxIdPacket"/></param>
        public bool Equals(SfxIdPacket other)
        {
            return this.Type == other.Type && this.IdSfxHit == other.IdSfxHit && this.Type == other.Type && this.Skill == other.Skill && this.MaxDamageCount == other.MaxDamageCount;
        }
    }
}