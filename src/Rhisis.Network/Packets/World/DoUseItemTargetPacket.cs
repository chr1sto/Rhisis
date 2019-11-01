using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="DoUseItemTargetPacket"/> structure.
    /// </summary>
    public struct DoUseItemTargetPacket : IEquatable<DoUseItemTargetPacket>
    {
        /// <summary>
        /// Gets the material.
        /// </summary>
        public uint Material { get; set; }

        /// <summary>
        /// Gets the target.
        /// </summary>
        public uint Target { get; set; }

        /// <summary>
        /// Creates a new <see cref="DoUseItemTargetPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public DoUseItemTargetPacket(INetPacketStream packet)
        {
            this.Material = packet.Read<uint>();
            this.Target = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="DoUseItemTargetPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="DoUseItemTargetPacket"/></param>
        public bool Equals(DoUseItemTargetPacket other)
        {
            return this.Material == other.Material && this.Target == other.Target;
        }
    }
}