using System;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Party
{
    /// <summary>
    /// Defines the <see cref="PartyAddMemberPacket"/> structure.
    /// </summary>
    public struct PartyAddMemberPacket : IEquatable<PartyAddMemberPacket>
    {
        /// <summary>
        /// Gets the leader id.
        /// </summary>
        public uint LeaderId { get; set; }

        /// <summary>
        /// Gets the leader level.
        /// </summary>
        public int LeaderLevel { get; set; }

        /// <summary>
        /// Gets the leader job.
        /// </summary>
        public int LeaderJob { get; set; }

        /// <summary>
        /// Gets the leader sex.
        /// </summary>
        public uint LeaderSex { get; set; }

        /// <summary>
        /// Gets the member id.
        /// </summary>
        public uint MemberId { get; set; }

        /// <summary>
        /// Gets the member level.
        /// </summary>
        public int MemberLevel { get; set; }
        
        /// <summary>
        /// Gets the member job.
        /// </summary>
        public int MemberJob { get; set; }

        /// <summary>
        /// Gets the member sex.
        /// </summary>
        public uint MemberSex { get; set; }

        /// <summary>
        /// Creates a new <see cref="PartyAddMemberPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public PartyAddMemberPacket(INetPacketStream packet)
        {
            LeaderId = packet.Read<uint>();
            LeaderLevel = packet.Read<int>();
            LeaderJob = packet.Read<int>();
            LeaderSex = packet.Read<uint>();
            MemberId = packet.Read<uint>();
            MemberLevel = packet.Read<int>();
            MemberJob = packet.Read<int>();
            MemberSex = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="PartyAddMemberPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PartyAddMemberPacket"/></param>
        public bool Equals(PartyAddMemberPacket other)
        {
            return LeaderId == other.LeaderId && LeaderLevel == other.LeaderLevel &&
                   LeaderJob == other.LeaderJob && LeaderSex == other.LeaderSex &&
                   MemberId == other.MemberId && MemberLevel == other.MemberLevel &&
                   MemberJob == other.MemberJob && MemberSex == other.MemberSex;
        }
    }
}