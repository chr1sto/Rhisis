using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.GuildCombat
{
    /// <summary>
    /// Defines the <see cref="GetPenyaPlayerGuildCombatPacket"/> structure.
    /// </summary>
    public struct GetPenyaPlayerGuildCombatPacket : IEquatable<GetPenyaPlayerGuildCombatPacket>
    {
        /// <summary>
        /// Creates a new <see cref="GetPenyaPlayerGuildCombatPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public GetPenyaPlayerGuildCombatPacket(INetPacketStream packet)
        {
        }

        /// <summary>
        /// Compares two <see cref="GetPenyaPlayerGuildCombatPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="GetPenyaPlayerGuildCombatPacket"/></param>
        public bool Equals(GetPenyaPlayerGuildCombatPacket other)
        {
            return true;
        }
    }
}