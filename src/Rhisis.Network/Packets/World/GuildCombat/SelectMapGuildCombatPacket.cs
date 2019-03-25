using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.GuildCombat
{
    /// <summary>
    /// Defines the <see cref="SelectMapGuildCombatPacket"/> structure.
    /// </summary>
    public struct SelectMapGuildCombatPacket : IEquatable<SelectMapGuildCombatPacket>
    {
        /// <summary>
        /// Gets the map id.
        /// </summary>
        public int Map { get; set; }
        /// <summary>
        /// Creates a new <see cref="SelectMapGuildCombatPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public SelectMapGuildCombatPacket(INetPacketStream packet)
        {
            this.Map = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="SelectMapGuildCombatPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="SelectMapGuildCombatPacket"/></param>
        public bool Equals(SelectMapGuildCombatPacket other)
        {
            return this.Map == other.Map;
        }
    }
}