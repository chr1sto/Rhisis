using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.GuildCombat
{
    /// <summary>
    /// Defines the <see cref="OutGuildCombatPacket"/> structure.
    /// </summary>
    public struct OutGuildCombatPacket : IEquatable<OutGuildCombatPacket>
    {
        /// <summary>
        /// Creates a new <see cref="OutGuildCombatPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public OutGuildCombatPacket(INetPacketStream packet)
        {
        }

        /// <summary>
        /// Compares two <see cref="OutGuildCombatPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="OutGuildCombatPacket"/></param>
        public bool Equals(OutGuildCombatPacket other)
        {
            return true;
        }
    }
}