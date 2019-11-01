using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ExpBoxInfoPacket"/> structure.
    /// </summary>
    public struct ExpBoxInfoPacket : IEquatable<ExpBoxInfoPacket>
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; set; }

        /// <summary>
        /// Gets the object id.
        /// </summary>
        public uint ObjectId { get; set; }

        /// <summary>
        /// Creates a new <see cref="ExpBoxInfoPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ExpBoxInfoPacket(INetPacketStream packet)
        {
            this.PlayerId = packet.Read<uint>();
            this.ObjectId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="ExpBoxInfoPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ExpBoxInfoPacket"/></param>
        public bool Equals(ExpBoxInfoPacket other)
        {
            return this.PlayerId == other.PlayerId && this.ObjectId == other.ObjectId;
        }
    }
}