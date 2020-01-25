using System;
using Sylver.Network.Data;

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
            SourcePlayerId = packet.Read<uint>();
            DestinationPlayerId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="DuelYesPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="DuelYesPacket"/></param>
        public bool Equals(DuelYesPacket other)
        {
            return SourcePlayerId == other.SourcePlayerId && DestinationPlayerId == other.DestinationPlayerId;
        }
    }
}