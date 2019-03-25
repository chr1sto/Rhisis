using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.Party
{
    /// <summary>
    /// Defines the <see cref="PartyChangeItemModePacket"/> structure.
    /// </summary>
    public struct PartyChangeItemModePacket : IEquatable<PartyChangeItemModePacket>
    {

        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; set; }

        /// <summary>
        /// Gets the item mode.
        /// </summary>
        public int ItemMode { get; set; }

        /// <summary>
        /// Creates a new <see cref="PartyChangeItemModePacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PartyChangeItemModePacket(INetPacketStream packet)
        {
            this.PlayerId = packet.Read<uint>();
            this.ItemMode = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="PartyChangeItemModePacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PartyChangeItemModePacket"/></param>
        public bool Equals(PartyChangeItemModePacket other)
        {
            return this.PlayerId == other.PlayerId && this.ItemMode == other.ItemMode;
        }
    }
}