using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.Bank
{
    /// <summary>
    /// Defines the <see cref="CloseBankWindowPacket"/> structure.
    /// </summary>
    public struct CloseBankWindowPacket : IEquatable<CloseBankWindowPacket>
    {
        /// <summary>
        /// Creates a new <see cref="CloseBankWindowPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public CloseBankWindowPacket(INetPacketStream packet)
        {
        }

        /// <summary>
        /// Compares two <see cref="CloseBankWindowPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="CloseBankWindowPacket"/></param>
        public bool Equals(CloseBankWindowPacket other)
        {
            return true;
        }
    }
}