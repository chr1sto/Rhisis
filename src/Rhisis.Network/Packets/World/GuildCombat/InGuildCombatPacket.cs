using System;
using Sylver.Network.Data;
using Rhisis.Core.Data;

namespace Rhisis.Network.Packets.World.GuildCombat
{
    /// <summary>
    /// Defines the <see cref="InGuildCombatPacket"/> structure.
    /// </summary>
    public struct InGuildCombatPacket : IEquatable<InGuildCombatPacket>
    {
        /// <summary>
        /// Gets the guild combat type.
        /// </summary>
        public GuildCombatType GuildCombatType { get; }

        /// <summary>
        /// Gets the guild combat gold amount.
        /// </summary>
        public uint? Penya { get; }

        /// <summary>
        /// Creates a new <see cref="InGuildCombatPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public InGuildCombatPacket(INetPacketStream packet)
        {
            GuildCombatType = (GuildCombatType)packet.Read<int>();
            if (GuildCombatType == GuildCombatType.GC_IN_APP)
                Penya = packet.Read<uint>();
            else
                Penya = null;
        }

        /// <summary>
        /// Compares two <see cref="InGuildCombatPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="InGuildCombatPacket"/></param>
        public bool Equals(InGuildCombatPacket other)
        {
            return GuildCombatType == other.GuildCombatType && Penya == other.Penya;
        }
    }
}