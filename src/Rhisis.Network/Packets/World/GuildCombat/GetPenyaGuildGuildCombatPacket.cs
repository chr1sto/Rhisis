using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.GuildCombat
{
    /// <summary>
    /// Defines the <see cref="GetPenyaGuildGuildCombatPacket"/> structure.
    /// </summary>
    public struct GetPenyaGuildGuildCombatPacket : IEquatable<GetPenyaGuildGuildCombatPacket>
    {
        /// <summary>
        /// Creates a new <see cref="GetPenyaGuildGuildCombatPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public GetPenyaGuildGuildCombatPacket(INetPacketStream packet)
        {
        }

        /// <summary>
        /// Compares two <see cref="GetPenyaGuildGuildCombatPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="GetPenyaGuildGuildCombatPacket"/></param>
        public bool Equals(GetPenyaGuildGuildCombatPacket other)
        {
            return true;
        }
    }
}