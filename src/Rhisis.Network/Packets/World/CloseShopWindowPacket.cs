using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="CloseShopWindowPacket"/> structure.
    /// </summary>
    public struct CloseShopWindowPacket : IEquatable<CloseShopWindowPacket>
    {
        /// <summary>
        /// Creates a new <see cref="CloseShopWindowPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public CloseShopWindowPacket(INetPacketStream packet)
        {
        }

        /// <summary>
        /// Compares two <see cref="CloseShopWindowPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="CloseShopWindowPacket"/></param>
        public bool Equals(CloseShopWindowPacket other)
        {
            return true;
        }
    }
}