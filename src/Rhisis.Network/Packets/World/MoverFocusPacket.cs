using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="MoverFocusPacket"/> structure.
    /// </summary>
    public struct MoverFocusPacket : IEquatable<MoverFocusPacket>
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; set; }

        /// <summary>
        /// Creates a new <see cref="MoverFocusPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public MoverFocusPacket(INetPacketStream packet)
        {
            this.PlayerId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="MoverFocusPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="MoverFocusPacket"/></param>
        public bool Equals(MoverFocusPacket other)
        {
            return this.PlayerId == other.PlayerId;
        }
    }
}