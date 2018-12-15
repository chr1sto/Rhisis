using System;
using Ether.Network.Packets;

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
            this.LeaderId = packet.Read<uint>();
            this.LeaderLevel = packet.Read<int>();
            this.LeaderJob = packet.Read<int>();
            this.LeaderSex = packet.Read<uint>();
            this.MemberId = packet.Read<uint>();
            this.MemberLevel = packet.Read<int>();
            this.MemberJob = packet.Read<int>();
            this.MemberSex = packet.Read<uint>();
        }

        /// <summary>
        /// Compares two <see cref="PartyAddMemberPacket"/>.
        /// </summary>
        /// <param name="other">Other <see cref="PartyAddMemberPacket"/></param>
        public bool Equals(PartyAddMemberPacket other)
        {
            return this.LeaderId == other.LeaderId && this.LeaderLevel == other.LeaderLevel &&
                   this.LeaderJob == other.LeaderJob && this.LeaderSex == other.LeaderSex &&
                   this.MemberId == other.MemberId && this.MemberLevel == other.MemberLevel &&
                   this.MemberJob == other.MemberJob && this.MemberSex == other.MemberSex;
        }
    }
}