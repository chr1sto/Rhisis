using Rhisis.Network.Packets.World.Party;
using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.Party.EventArgs
{
    public class PartyAddMemberEventArgs : SystemEventArgs
    {
        /// <summary>
        /// Gets the leader id.
        /// </summary>
        public uint LeaderId { get; }

        /// <summary>
        /// Gets the leader level.
        /// </summary>
        public int LeaderLevel { get; }

        /// <summary>
        /// Gets the leader job.
        /// </summary>
        public int LeaderJob { get; }

        /// <summary>
        /// Gets the leader sex.
        /// </summary>
        public uint LeaderSex { get; }

        /// <summary>
        /// Gets the member id.
        /// </summary>
        public uint MemberId { get; }

        /// <summary>
        /// Gets the member level.
        /// </summary>
        public int MemberLevel { get; }

        /// <summary>
        /// Gets the member job.
        /// </summary>
        public int MemberJob { get; }

        /// <summary>
        /// Gets the member sex.
        /// </summary>
        public uint MemberSex { get; }

        /// <summary>
        /// Creates a <see cref="PartyAddMemberEventArgs"/> instance.
        /// </summary>
        /// <param name="leaderId"></param>
        /// <param name="leaderLevel"></param>
        /// <param name="leaderJob"></param>
        /// <param name="leaderSex"></param>
        /// <param name="memberId"></param>
        /// <param name="memberLevel"></param>
        /// <param name="memberJob"></param>
        /// <param name="memberSex"></param>
        public PartyAddMemberEventArgs(uint leaderId, int leaderLevel, int leaderJob, uint leaderSex, uint memberId, int memberLevel, int memberJob, uint memberSex)
        {
            LeaderId = leaderId;
            LeaderLevel = leaderLevel;
            LeaderJob = leaderJob;
            LeaderSex = leaderSex;
            MemberId = memberId;
            MemberLevel = memberLevel;
            MemberJob = memberJob;
            MemberSex = memberSex;
        }

        /// <summary>
        /// Creates a <see cref="PartyAddMemberEventArgs"/> instance.
        /// </summary>
        /// <param name="packet"></param>
        public PartyAddMemberEventArgs(PartyAddMemberPacket packet) :
            this(packet.LeaderId, packet.LeaderLevel, packet.LeaderJob, packet.LeaderSex, packet.MemberId, packet.MemberLevel, packet.MemberJob, packet.MemberSex)
        {
        }

        public override bool CheckArguments()
        {
            return LeaderId > 0 && MemberId > 0 && LeaderId != MemberId;
        }
    }
}
