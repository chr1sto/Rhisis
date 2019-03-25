using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.GuildCombat
{
    /// <summary>
    /// Defines the <see cref="TeleGuildCombatPacket"/> structure.
    /// </summary>
    public struct TeleGuildCombatPacket : IEquatable<TeleGuildCombatPacket>
    {
        /// <summary>
        /// Creates a new <see cref="TeleGuildCombatPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public TeleGuildCombatPacket(INetPacketStream packet)
        {
        }

        /// <summary>
        /// Compares two <see cref="TeleGuildCombatPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="TeleGuildCombatPacket"/></param>
        public bool Equals(TeleGuildCombatPacket other)
        {
            return true;
        }
    }
}