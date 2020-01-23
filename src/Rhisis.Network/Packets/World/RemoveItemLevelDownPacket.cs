using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="RemoveItemLevelDownPacket"/> structure.
    /// </summary>
    public struct RemoveItemLevelDownPacket : IEquatable<RemoveItemLevelDownPacket>
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Creates a new <see cref="RemoveItemLevelDownPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public RemoveItemLevelDownPacket(INetPacketStream packet)
        {
            Id = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="RemoveItemLevelDownPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="RemoveItemLevelDownPacket"/></param>
        public bool Equals(RemoveItemLevelDownPacket other)
        {
            return Id == other.Id;
        }
    }
}