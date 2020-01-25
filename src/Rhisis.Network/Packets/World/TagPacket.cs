using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="TagPacket"/> structure.
    /// </summary>
    public struct TagPacket : IEquatable<TagPacket>
    {
        /// <summary>
        /// Gets the target id.
        /// </summary>
        public uint TargetId { get; set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Creates a new <see cref="TagPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public TagPacket(INetPacketStream packet)
        {
            TargetId = packet.Read<uint>();
            Message = packet.Read<string>();
        }

        /// <summary>
        /// Compares two <see cref="TagPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="TagPacket"/></param>
        public bool Equals(TagPacket other)
        {
            return TargetId == other.TargetId && Message == other.Message;
        }
    }
}