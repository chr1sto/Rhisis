using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Party
{
    public class PartySkillUsePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; private set; }

        /// <summary>
        /// Gets the skill id.
        /// </summary>
        public int SkillId { get; private set; }


        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            PlayerId = packet.Read<uint>();
            SkillId = packet.Read<int>();
        }
    }
}