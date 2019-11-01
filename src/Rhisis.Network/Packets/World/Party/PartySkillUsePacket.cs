using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Party
{
    /// <summary>
    /// Defines the <see cref="PartySkillUsePacket"/> structure.
    /// </summary>
    public struct PartySkillUsePacket : IEquatable<PartySkillUsePacket>
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; set; }

        /// <summary>
        /// Gets the skill id.
        /// </summary>
        public int SkillId { get; set; }


        /// <summary>
        /// Creates a new <see cref="PartySkillUsePacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PartySkillUsePacket(INetPacketStream packet)
        {
            this.PlayerId = packet.Read<uint>();
            this.SkillId = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="PartySkillUsePacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PartySkillUsePacket"/></param>
        public bool Equals(PartySkillUsePacket other)
        {
            return this.PlayerId == other.PlayerId && this.SkillId == other.SkillId;
        }
    }
}