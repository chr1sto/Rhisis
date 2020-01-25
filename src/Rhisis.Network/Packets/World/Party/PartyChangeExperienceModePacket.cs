using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Party
{
    /// <summary>
    /// Defines the <see cref="PartyChangeExperienceModePacket"/> structure.
    /// </summary>
    public struct PartyChangeExperienceModePacket : IEquatable<PartyChangeExperienceModePacket>
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; set; }

        /// <summary>
        /// Gets the experience mode.
        /// </summary>
        public int ExperienceMode { get; set; }

        /// <summary>
        /// Creates a new <see cref="PartyChangeExperienceModePacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PartyChangeExperienceModePacket(INetPacketStream packet)
        {
            PlayerId = packet.Read<uint>();
            ExperienceMode = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="PartyChangeExperienceModePacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PartyChangeExperienceModePacket"/></param>
        public bool Equals(PartyChangeExperienceModePacket other)
        {
            return PlayerId == other.PlayerId && ExperienceMode == other.ExperienceMode;
        }
    }
}