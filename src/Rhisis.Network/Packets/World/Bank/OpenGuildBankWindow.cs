using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.Bank
{
    /// <summary>
    /// Defines the <see cref="OpenGuildBankWindow"/> structure.
    /// </summary>
    public struct OpenGuildBankWindow : IEquatable<OpenGuildBankWindow>
    {
        /// <summary>
        /// Creates a new <see cref="OpenGuildBankWindow"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public OpenGuildBankWindow(INetPacketStream packet)
        {
        }

        /// <summary>
        /// Compares two <see cref="OpenGuildBankWindow"/>.
        /// </summary>
        /// <param name="other">Other <see cref="OpenGuildBankWindow"/></param>
        public bool Equals(OpenGuildBankWindow other)
        {
            return true;
        }
    }
}