using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="ActMsgPacket"/> structure.
    /// </summary>
    public struct ActMsgPacket : IEquatable<ActMsgPacket>
    {
        /// <summary>
        /// Gets the message.
        /// </summary>
        public uint Message { get; set; }

        /// <summary>
        /// Gets the first parameter.
        /// </summary>
        public int Parameter1 { get; set; }

        /// <summary>
        /// Gets the second parameter.
        /// </summary>
        public int Parameter2 { get; set; }

        /// <summary>
        /// Creates a new <see cref="ActMsgPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public ActMsgPacket(INetPacketStream packet)
        {
            this.Message = packet.Read<uint>();
            this.Parameter1 = packet.Read<int>();
            this.Parameter2 = packet.Read<int>();
        }

        /// <summary>
        /// Compares two <see cref="ActMsgPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="ActMsgPacket"/></param>
        public bool Equals(ActMsgPacket other)
        {
            return true;
        }
    }
}