using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.GuildCombat
{
    /// <summary>
    /// Defines the <see cref="PlayerPointGuildCombatPacket"/> structure.
    /// </summary>
    public struct PlayerPointGuildCombatPacket : IEquatable<PlayerPointGuildCombatPacket>
    {
        /// <summary>
        /// Creates a new <see cref="PlayerPointGuildCombatPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PlayerPointGuildCombatPacket(INetPacketStream packet)
        {
        }

        /// <summary>
        /// Compares two <see cref="PlayerPointGuildCombatPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PlayerPointGuildCombatPacket"/></param>
        public bool Equals(PlayerPointGuildCombatPacket other)
        {
            return true;
        }
    }
}