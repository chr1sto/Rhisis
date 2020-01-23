using System;
using Sylver.Network.Data;

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
            SourcePlayerId = packet.Read<uint>();
            DestinationPlayerId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="DuelRequestPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="DuelRequestPacket"/></param>
        public bool Equals(DuelRequestPacket other)
        {
            return SourcePlayerId == other.SourcePlayerId && DestinationPlayerId == other.DestinationPlayerId;
        }
    }
}