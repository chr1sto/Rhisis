using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Duel
{
    /// <summary>
    /// Defines the <see cref="PartyDuelNoPacket"/> structure.
    /// </summary>
    public struct PartyDuelNoPacket : IEquatable<PartyDuelNoPacket>
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; set; }

        /// <summary>
        /// Creates a new <see cref="DuelNoPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PartyDuelNoPacket(INetPacketStream packet)
        {
            PlayerId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="PartyDuelNoPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PartyDuelNoPacket"/></param>
        public bool Equals(PartyDuelNoPacket other)
        {
            return PlayerId == other.PlayerId;
        }
    }
}