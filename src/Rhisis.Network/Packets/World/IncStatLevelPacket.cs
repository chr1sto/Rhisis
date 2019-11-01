using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="IncStatLevelPacket"/> structure.
    /// </summary>
    public struct IncStatLevelPacket : IEquatable<IncStatLevelPacket>
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public byte Id { get; set; }

        /// <summary>
        /// Creates a new <see cref="IncStatLevelPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public IncStatLevelPacket(INetPacketStream packet)
        {
            this.Id = packet.Read<byte>();
        }

        /// <summary>
        /// Compares two <see cref="IncStatLevelPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="IncStatLevelPacket"/></param>
        public bool Equals(IncStatLevelPacket other)
        {
            return this.Id == other.Id;
        }
    }
}