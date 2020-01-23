using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Duel
{
    /// <summary>
    /// Defines the <see cref="DuelNoPacket"/> structure.
    /// </summary>
    public struct DuelNoPacket : IEquatable<DuelNoPacket>
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; set; }

        /// <summary>
        /// Creates a new <see cref="DuelNoPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public DuelNoPacket(INetPacketStream packet)
        {
            PlayerId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="DuelNoPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="DuelNoPacket"/></param>
        public bool Equals(DuelNoPacket other)
        {
            return PlayerId == other.PlayerId;
        }
    }
}