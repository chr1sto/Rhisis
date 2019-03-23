using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.GuildCombat
{
    /// <summary>
    /// Defines the <see cref="GetItemGuildCombatPacket"/> structure.
    /// </summary>
    public struct GetItemGuildCombatPacket : IEquatable<GetItemGuildCombatPacket>
    {
        /// <summary>
        /// Creates a new <see cref="GetItemGuildCombatPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public GetItemGuildCombatPacket(INetPacketStream packet)
        {
        }

        /// <summary>
        /// Compares two <see cref="GetItemGuildCombatPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="GetItemGuildCombatPacket"/></param>
        public bool Equals(GetItemGuildCombatPacket other)
        {
            return true;
        }
    }
}