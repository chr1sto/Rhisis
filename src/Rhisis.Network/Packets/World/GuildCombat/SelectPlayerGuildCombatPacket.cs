using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.GuildCombat
{
    /// <summary>
    /// Defines the <see cref="SelectPlayerGuildCombatPacket"/> structure.
    /// </summary>
    public struct SelectPlayerGuildCombatPacket : IEquatable<SelectPlayerGuildCombatPacket>
    {
        /// <summary>
        /// Gets the window.
        /// </summary>
        public bool Window { get; set; }

        /// <summary>
        /// Gets the defender id.
        /// </summary>
        public uint? DefenderId { get; set; }

        /// <summary>
        /// Gets the SelectPlayer size.
        /// </summary>
        public int? Size { get; set; }

        /// <summary>
        /// Gets the players to select.
        /// </summary>
        public uint?[] SelectPlayer { get; set; }

        /// <summary>
        /// Creates a new <see cref="SelectPlayerGuildCombatPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public SelectPlayerGuildCombatPacket(INetPacketStream packet)
        {
            this.Window = packet.Read<int>() == 1;
            if (!this.Window)
            {
                DefenderId = packet.Read<uint>();
                Size = packet.Read<int>();
                SelectPlayer = new uint?[Size.Value];
                for (int i = 0; i < Size.Value; i++)
                    SelectPlayer[i] = packet.Read<uint>();
            }
            else
            {
                DefenderId = null;
                Size = null;
                SelectPlayer = null;
            }
        }

        /// <summary>
        /// Compares two <see cref="SelectPlayerGuildCombatPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="SelectPlayerGuildCombatPacket"/></param>
        public bool Equals(SelectPlayerGuildCombatPacket other)
        {
            return this.Window == other.Window && this.DefenderId == other.DefenderId && this.Size == other.Size && this.SelectPlayer == other.SelectPlayer;
        }
    }
}