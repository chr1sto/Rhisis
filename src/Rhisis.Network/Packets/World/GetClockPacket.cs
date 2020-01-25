using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    /// <summary>
    /// Defines the <see cref="GetClockPacket"/> structure.
    /// </summary>
    public struct GetClockPacket : IEquatable<GetClockPacket>
    {
        /// <summary>
        /// Gets the SetBaseOfClient.
        /// </summary>
        public byte SetBaseOfClient { get; set; }

        /// <summary>
        /// Gets the client time
        /// </summary>
        public uint ClientTime { get; set; }

        /// <summary>
        /// Creates a new <see cref="GetClockPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public GetClockPacket(INetPacketStream packet)
        {
            SetBaseOfClient = packet.Read<byte>();
            ClientTime = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="GetClockPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="GetClockPacket"/></param>
        public bool Equals(GetClockPacket other)
        {
            return SetBaseOfClient == other.SetBaseOfClient && ClientTime == other.ClientTime;
        }
    }
}