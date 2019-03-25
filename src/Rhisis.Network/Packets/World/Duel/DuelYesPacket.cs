using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.Duel
{
    /// <summary>
    /// Defines the <see cref="DuelYesPacket"/> structure.
    /// </summary>
    public struct DuelYesPacket : IEquatable<DuelYesPacket>
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
        /// Creates a new <see cref="DuelYesPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public DuelYesPacket(INetPacketStream packet)
        {
            this.SourcePlayerId = packet.Read<uint>();
            this.DestinationPlayerId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="DuelYesPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="DuelYesPacket"/></param>
        public bool Equals(DuelYesPacket other)
        {
            return this.SourcePlayerId == other.SourcePlayerId && this.DestinationPlayerId == other.DestinationPlayerId;
        }
    }
}