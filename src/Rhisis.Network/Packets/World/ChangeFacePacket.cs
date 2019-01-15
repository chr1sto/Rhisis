using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ChangeFacePacket"/> structure.
    /// </summary>
    public struct ChangeFacePacket : IEquatable<ChangeFacePacket>
    {
        /// <summary>
        /// Gets the object id.
        /// </summary>
        public uint ObjectId { get; set; }

        /// <summary>
        /// Gets the face number.
        /// </summary>
        public uint FaceNumber { get; set; }

        /// <summary>
        /// Gets the cost.
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Gets if a coupon will be used.
        /// </summary>
        public bool UseCoupon { get; set; }

        /// <summary>
        /// Creates a new <see cref="ChangeFacePacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ChangeFacePacket(INetPacketStream packet)
        {
            this.ObjectId = packet.Read<uint>();
            this.FaceNumber = packet.Read<uint>();
            this.Cost = packet.Read<int>();
            this.UseCoupon = Convert.ToBoolean(packet.Read<int>());
        }

        /// <summary>
        /// Compares two <see cref="ChangeFacePacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ChangeFacePacket"/></param>
        public bool Equals(ChangeFacePacket other)
        {
            return this.ObjectId == other.ObjectId && this.FaceNumber == other.FaceNumber && this.Cost == other.Cost && this.UseCoupon == other.UseCoupon;
        }
    }
}