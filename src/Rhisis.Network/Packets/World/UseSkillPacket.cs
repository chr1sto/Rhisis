using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class UseSkillPacket : IPacketDeserializer
    {

        public ushort Type { get; private set; }

        public ushort Id { get; private set; }

        public uint ObjectId { get; private set; }

        public int UseSkill { get; private set; }

        public bool Control { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Type = packet.Read<ushort>();
            Id = packet.Read<ushort>();
            ObjectId = packet.Read<uint>();
            UseSkill = packet.Read<int>();
            Control = packet.Read<int>() == 1;
        }
    }
}