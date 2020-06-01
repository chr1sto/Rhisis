using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class ChangeFacePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the object id.
        /// </summary>
        public uint ObjectId { get; private set; }

        /// <summary>
        /// Gets the face number.
        /// </summary>
        public uint FaceNumber { get; private set; }

        /// <summary>
        /// Gets the cost.
        /// </summary>
        public int Cost { get; private set; }

        /// <summary>
        /// Gets if a coupon will be used.
        /// </summary>
        public bool UseCoupon { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            ObjectId = packet.Read<uint>();
            FaceNumber = packet.Read<uint>();
            Cost = packet.Read<int>();
            UseCoupon = Convert.ToBoolean(packet.Read<int>());
        }
    }
}