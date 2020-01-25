using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Party
{
    public class PartyChangeLeaderPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the leader id.
        /// </summary>
        public uint LeaderId { get; private set; }

        /// <summary>
        /// Gets the new leader id.
        /// </summary>
        public uint NewLeaderId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            LeaderId = packet.Read<uint>();
            NewLeaderId = packet.Read<uint>();
        }
    }
}