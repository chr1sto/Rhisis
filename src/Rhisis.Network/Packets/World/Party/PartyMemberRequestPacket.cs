using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Party
{
    /// <summary>
    /// Defines the <see cref="PartyMemberRequestPacket"/> structure.
    /// </summary>
    public struct PartyMemberRequestPacket : IEquatable<PartyMemberRequestPacket>
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; set; }

        /// <summary>
        /// Gets the member id.
        /// </summary>
        public uint MemberId { get; set; }

        /// <summary>
        /// Gets if it's a troup.
        /// </summary>
        public bool Troup { get; set; }

        /// <summary>
        /// Creates a new <see cref="PartyMemberRequestPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PartyMemberRequestPacket(INetPacketStream packet)
        {
            PlayerId = packet.Read<uint>();
            MemberId = packet.Read<uint>();
            Troup = packet.Read<int>() == 1;
        }

        /// <summary>
        /// Compares two <see cref="PartyMemberRequestPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PartyMemberRequestPacket"/></param>
        public bool Equals(PartyMemberRequestPacket other)
        {
            return PlayerId == other.PlayerId && MemberId == other.MemberId && Troup == other.Troup;
        }
    }
}