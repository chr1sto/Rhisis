using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class SetHairPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the hair id.
        /// </summary>
        public byte HairId { get; private set; }

        /// <summary>
        /// Gets the red color.
        /// </summary>
        public byte R { get; private set; }

        /// <summary>
        /// Gets the green color.
        /// </summary>
        public byte G { get; private set; }

        /// <summary>
        /// Gets the blue color.
        /// </summary>
        public byte B { get; private set; }

        /// <summary>
        /// Gets if a coupon will be used.
        /// </summary>
        public bool UseCoupon { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            HairId = packet.Read<byte>();
            R = packet.Read<byte>();
            G = packet.Read<byte>();
            B = packet.Read<byte>();
            UseCoupon = Convert.ToBoolean(packet.Read<int>());
        }
    }
}
