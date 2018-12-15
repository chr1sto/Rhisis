using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="MoveBankItemPacket"/> structure.
    /// </summary>
    public struct MoveBankItemPacket : IEquatable<MoveBankItemPacket>
    {
        /// <summary>
        /// Gets the source index.
        /// </summary>
        public int SourceIndex { get; set; }

        /// <summary>
        /// Gets the destination index.
        /// </summary>
        public int Destinationindex { get; set; }

        /// <summary>
        /// Creates a new <see cref="MoveBankItemPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public MoveBankItemPacket(INetPacketStream packet)
        {
            this.SourceIndex = packet.Read<int>();
            this.Destinationindex = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="MoveBankItemPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="MoveBankItemPacket"/></param>
        public bool Equals(MoveBankItemPacket other)
        {
            return this.SourceIndex == other.SourceIndex && this.Destinationindex == other.Destinationindex;
        }
    }
}