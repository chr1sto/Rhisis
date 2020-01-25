using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Party
{
    /// <summary>
    /// Defines the <see cref="PartyRemoveMemberPacket"/> structure.
    /// </summary>
    public struct PartyRemoveMemberPacket : IEquatable<PartyRemoveMemberPacket>
    {

        public uint LeaderId { get; set; }

        public uint MemberId { get; set; }

        /// <summary>
        /// Creates a new <see cref="PartyRemoveMemberPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PartyRemoveMemberPacket(INetPacketStream packet)
        {
            LeaderId = packet.Read<uint>();
            MemberId = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="PartyRemoveMemberPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PartyRemoveMemberPacket"/></param>
        public bool Equals(PartyRemoveMemberPacket other)
        {
            return LeaderId == other.LeaderId && MemberId == other.MemberId;
        }
    }
}