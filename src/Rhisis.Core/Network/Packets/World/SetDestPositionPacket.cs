using Ether.Network.Packets;
using System;

namespace Rhisis.Core.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="SetDestPositionPacket"/> structure.
    /// </summary>
    public struct SetDestPositionPacket : IEquatable<SetDestPositionPacket>
    {
        /// <summary>
        /// Gets the X coordinate.
        /// </summary>
        public float X { get; }

        /// <summary>
        /// Gets the Y coordinate.
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// Gets the Z coordinate.
        /// </summary>
        public float Z { get; }

        /// <summary>
        /// Gets the forward state.
        /// </summary>
        public byte Forward { get; }

        /// <summary>
        /// Creates a new <see cref="SetDestPositionPacket"/> instance.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public SetDestPositionPacket(INetPacketStream packet)
        {
            this.X = packet.Read<float>();
            this.Y = packet.Read<float>();
            this.Z = packet.Read<float>();
            this.Forward = packet.Read<byte>();
        }

        /// <summary>
        /// Compares two <see cref="SetDestPositionPacket"/> objects.
        /// </summary>
        /// <param name="other">Other <see cref="SetDestPositionPacket"/></param>
        /// <returns></returns>
        public bool Equals(SetDestPositionPacket other)
        {
            throw new NotImplementedException();
        }
    }
}
