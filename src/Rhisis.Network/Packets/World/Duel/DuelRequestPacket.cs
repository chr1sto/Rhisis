using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.Duel
{
    /// <summary>
    /// Defines the <see cref="DuelRequestPacket"/> structure.
    /// </summary>
    public struct DuelRequestPacket : IEquatable<DuelRequestPacket>
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
        /// Creates a new <see cref="DuelRequestPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public DuelRequestPacket(INetPacketStream packet)
        {
            this.SourcePlayerId = packet.Read<uint>();
            this.DestinationPlayerId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="DuelRequestPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="DuelRequestPacket"/></param>
        public bool Equals(DuelRequestPacket other)
        {
            return this.SourcePlayerId == other.SourcePlayerId && this.DestinationPlayerId == other.DestinationPlayerId;
        }
    }
}