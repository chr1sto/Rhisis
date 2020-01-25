using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Party
{
    public class PartyAddMemberPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the leader id.
        /// </summary>
        public uint LeaderId { get; private set; }

        /// <summary>
        /// Gets the leader level.
        /// </summary>
        public int LeaderLevel { get; private set; }

        /// <summary>
        /// Gets the leader job.
        /// </summary>
        public int LeaderJob { get; private set; }

        /// <summary>
        /// Gets the leader sex.
        /// </summary>
        public uint LeaderSex { get; private set; }

        /// <summary>
        /// Gets the member id.
        /// </summary>
        public uint MemberId { get; private set; }

        /// <summary>
        /// Gets the member level.
        /// </summary>
        public int MemberLevel { get; private set; }
        
        /// <summary>
        /// Gets the member job.
        /// </summary>
        public int MemberJob { get; private set; }

        /// <summary>
        /// Gets the member sex.
        /// </summary>
        public uint MemberSex { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
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
    }
}