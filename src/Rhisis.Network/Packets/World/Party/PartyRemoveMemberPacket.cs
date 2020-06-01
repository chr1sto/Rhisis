using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Party
{
    public class PartyRemoveMemberPacket : IPacketDeserializer
    {

        public uint LeaderId { get; private set; }

        public uint MemberId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            LeaderId = packet.Read<uint>();
            MemberId = packet.Read<uint>();
        }
    }
}