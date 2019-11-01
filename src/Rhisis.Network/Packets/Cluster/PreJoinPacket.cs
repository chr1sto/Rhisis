using Sylver.Network.Data;

namespace Rhisis.Network.Packets.Cluster
{
    public class PreJoinPacket : IPacketDeserializer
    {
        public string Username { get; private set; }

        public int CharacterId { get; private set; }

        public string CharacterName { get; private set; }

        public int BankCode { get; private set; }

        public void Deserialize(INetPacketStream packet)
        {
            this.Username = packet.Read<string>();
            this.CharacterId = packet.Read<int>();
            this.CharacterName = packet.Read<string>();
            this.BankCode = packet.Read<int>();
        }
    }
}
