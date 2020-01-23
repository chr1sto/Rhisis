using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="IncJobLevelPacket"/> structure.
    /// </summary>
    public struct IncJobLevelPacket : IEquatable<IncJobLevelPacket>
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public byte Id { get; set; }

        /// <summary>
        /// Creates a new <see cref="IncJobLevelPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public IncJobLevelPacket(INetPacketStream packet)
        {
            Id = packet.Read<byte>();
        }

        /// <summary>
        /// Compares two <see cref="IncJobLevelPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="IncJobLevelPacket"/></param>
        public bool Equals(IncJobLevelPacket other)
        {
            return Id == other.Id;
        }
    }
}