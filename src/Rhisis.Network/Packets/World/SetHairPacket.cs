using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="SetHairPacket"/> structure.
    /// </summary>
    public struct SetHairPacket : IEquatable<SetHairPacket>
    {
        /// <summary>
        /// Gets the hair id.
        /// </summary>
        public byte HairId { get; set; }

        /// <summary>
        /// Gets the red color.
        /// </summary>
        public byte R { get; set; }

        /// <summary>
        /// Gets the green color.
        /// </summary>
        public byte G { get; set; }

        /// <summary>
        /// Gets the blue color.
        /// </summary>
        public byte B { get; set; }

        /// <summary>
        /// Gets if a coupon will be used.
        /// </summary>
        public bool UseCoupon { get; set; }

        /// <summary>
        /// Creates a new <see cref="SetHairPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public SetHairPacket(INetPacketStream packet)
        {
            this.HairId = packet.Read<byte>();
            this.R = packet.Read<byte>();
            this.G = packet.Read<byte>();
            this.B = packet.Read<byte>();
            this.UseCoupon = Convert.ToBoolean(packet.Read<int>());
        }

        /// <summary>
        /// Compares two <see cref="SetHairPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="SetHairPacket"/></param>
        public bool Equals(SetHairPacket other)
        {
            return this.HairId == other.HairId && this.R == other.R && this.G == other.G && this.B == other.B && this.UseCoupon == other.UseCoupon;
        }
    }
}
