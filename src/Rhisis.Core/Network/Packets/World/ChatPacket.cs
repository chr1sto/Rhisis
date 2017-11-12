using Ether.Network.Packets;
using System;

namespace Rhisis.Core.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ChatPacket"/> structure.
    /// </summary>
    public struct ChatPacket : IEquatable<ChatPacket>
    {
        /// <summary>
        /// Gets the chat message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Creates a new <see cref="ChatPacket"/> instance.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ChatPacket(NetPacketBase packet)
        {
            this.Message = packet.Read<string>();
        }

        /// <summary>
        /// Compares two <see cref="ChatPacket"/> objects.
        /// </summary>
        /// <param name="other">Other <see cref="ChatPacket"/></param>
        /// <returns></returns>
        public bool Equals(ChatPacket other) => this.Message.Equals(other.Message);
    }
}
