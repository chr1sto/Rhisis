using System;
using Ether.Network.Packets;

namespace Rhisis.Network.Packets.World.Packet
{
    /// <summary>
    /// Defines the <see cref="PartyChangeLeaderPacket"/> structure.
    /// </summary>
    public struct PartyChangeLeaderPacket : IEquatable<PartyChangeLeaderPacket>
    {
        /// <summary>
        /// Gets the leader id.
        /// </summary>
        public uint LeaderId { get; set; }

        /// <summary>
        /// Gets the new leader id.
        /// </summary>
        public uint NewLeaderId { get; set; }

        /// <summary>
        /// Creates a new <see cref="PartyChangeLeaderPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PartyChangeLeaderPacket(INetPacketStream packet)
        {
            this.LeaderId = packet.Read<uint>();
            this.NewLeaderId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="PartyChangeLeaderPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PartyChangeLeaderPacket"/></param>
        public bool Equals(PartyChangeLeaderPacket other)
        {
            return this.LeaderId == other.LeaderId && this.NewLeaderId == other.NewLeaderId;
        }
    }
}