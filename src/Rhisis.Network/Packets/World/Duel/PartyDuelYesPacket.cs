using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.Duel
{
    /// <summary>
    /// Defines the <see cref="PartyDuelYesPacket"/> structure.
    /// </summary>
    public struct PartyDuelYesPacket : IEquatable<PartyDuelYesPacket>
    {
        /// <summary>
        /// Gets the source player id.
        /// </summary>
        public uint SourcePlayerId { get; set; }

        /// <summary>
        /// Gets the destination player id.
        /// </summary>
        public uint DestinationPlayerId { get; set; }

        /// <summary>
        /// Creates a new <see cref="PartyDuelYesPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PartyDuelYesPacket(INetPacketStream packet)
        {
            this.SourcePlayerId = packet.Read<uint>();
            this.DestinationPlayerId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="PartyDuelYesPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PartyDuelYesPacket"/></param>
        public bool Equals(PartyDuelYesPacket other)
        {
            return this.SourcePlayerId == other.SourcePlayerId && this.DestinationPlayerId == other.DestinationPlayerId;
        }
    }
}