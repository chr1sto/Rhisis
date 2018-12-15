using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.GuildCombat
{
    /// <summary>
    /// Defines the <see cref="JoinGuildCombatPacket"/> structure.
    /// </summary>
    public struct JoinGuildCombatPacket : IEquatable<JoinGuildCombatPacket>
    {
        /// <summary>
        /// Creates a new <see cref="JoinGuildCombatPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public JoinGuildCombatPacket(INetPacketStream packet)
        {
        }

        /// <summary>
        /// Compares two <see cref="JoinGuildCombatPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="JoinGuildCombatPacket"/></param>
        public bool Equals(JoinGuildCombatPacket other)
        {
            return true;
        }
    }
}