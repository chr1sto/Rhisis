using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Describes the packet structure of the <see cref="PacketType.DOUSEITEM"/>.
    /// </summary>
    public struct DoUseItemPacket : IEquatable<DoUseItemPacket>
    {
        /// <summary>
        /// Gets or sets the Unique Item Id.
        /// </summary>
        public int UniqueItemId { get; }

        /// <summary>
        /// Gets or sets the object id.
        /// </summary>
        public int ObjectId { get; }

        /// <summary>
        /// Gets or sets the body part.
        /// </summary>
        public int Part { get; }

        /// <summary>
        /// Gets or sets the item flight speed.
        /// </summary>
        public float FlightSpeed { get; }

        /// <summary>
        /// Creates a new <see cref="DoUseItemPacket"/> instance.
        /// </summary>
        /// <param name="packet">Incoming packet stream.</param>
        public DoUseItemPacket(INetPacketStream packet)
        {
            this.UniqueItemId = (packet.Read<int>() >> 16) & 0xFFFF;
            this.ObjectId = packet.Read<int>();
            this.Part = packet.Read<int>();
            this.FlightSpeed = packet.Read<float>();
        }

        /// <summary>
        /// Compares two <see cref="DoUseItemPacket"/> objects.
        /// </summary>
        /// <param name="other">Other <see cref="DoUseItemPacket"/> object.</param>
        /// <returns>True if the same; false otherwise.</returns>
        public bool Equals(DoUseItemPacket other) 
            => (this.UniqueItemId, this.ObjectId, this.Part, this.FlightSpeed) == (other.UniqueItemId, other.ObjectId, other.Part, other.FlightSpeed);
    }
}