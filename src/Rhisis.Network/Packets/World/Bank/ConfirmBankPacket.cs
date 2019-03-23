using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.Bank
{
    /// <summary>
    /// Defines the <see cref="ConfirmBankPacket"/> structure.
    /// </summary>
    public struct ConfirmBankPacket : IEquatable<ConfirmBankPacket>
    {
        /// <summary>
        /// Creates a new <see cref="ConfirmBankPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ConfirmBankPacket(INetPacketStream packet)
        {
        }

        /// <summary>
        /// Compares two <see cref="ConfirmBankPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ConfirmBankPacket"/></param>
        public bool Equals(ConfirmBankPacket other)
        {
            return true;
        }
    }
}