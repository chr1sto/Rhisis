using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="KeepAlivePacket"/> structure.
    /// </summary>
    public struct KeepAlivePacket : IEquatable<KeepAlivePacket>
    {
        /// <summary>
        /// Creates a new <see cref="KeepAlivePacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public KeepAlivePacket(INetPacketStream packet)
        {
        }

        /// <summary>
        /// Compares two <see cref="KeepAlivePacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="KeepAlivePacket"/></param>
        public bool Equals(KeepAlivePacket other)
        {
            return true;
        }
    }
}