using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Bank
{
    /// <summary>
    /// Defines the <see cref="CloseGuildBankWindow"/> structure.
    /// </summary>
    public struct CloseGuildBankWindow : IEquatable<CloseGuildBankWindow>
    {
        /// <summary>
        /// Creates a new <see cref="CloseGuildBankWindow"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public CloseGuildBankWindow(INetPacketStream packet)
        {
        }

        /// <summary>
        /// Compares two <see cref="CloseGuildBankWindow"/>.
        /// </summary>
        /// <param name="other">Other <see cref="CloseGuildBankWindow"/></param>
        public bool Equals(CloseGuildBankWindow other)
        {
            return true;
        }
    }
}