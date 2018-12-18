using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="PlayerAnglePacket"/> structure.
    /// </summary>
    public struct PlayerAnglePacket : IEquatable<PlayerAnglePacket>
    {
        /// <summary>
        /// Gets the Angle.
        /// </summary>
        public float Angle { get; }

        /// <summary>
        /// Gets the X angle.
        /// </summary>
        public float AngleX { get; set; }

        /// <summary>
        /// Creates a new <see cref="PlayerAnglePacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PlayerAnglePacket(INetPacketStream packet)
        {
            this.Angle = packet.Read<float>();
            this.AngleX = packet.Read<float>();
        }

        /// <summary>
        /// Compares two <see cref="PlayerAnglePacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PlayerAnglePacket"/></param>
        public bool Equals(PlayerAnglePacket other)
        {
            return this.Angle == other.Angle &&
                   this.AngleX == other.AngleX;
        }
    }
}