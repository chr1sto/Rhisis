using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.GuildCombat
{
    /// <summary>
    /// Defines the <see cref="RequestStatusPacket"/> structure.
    /// </summary>
    public struct RequestStatusPacket : IEquatable<RequestStatusPacket>
    {
        /// <summary>
        /// Creates a new <see cref="RequestStatusPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public RequestStatusPacket(INetPacketStream packet)
        {
        }

        /// <summary>
        /// Compares two <see cref="RequestStatusPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="RequestStatusPacket"/></param>
        public bool Equals(RequestStatusPacket other)
        {
            return true;
        }
    }
}