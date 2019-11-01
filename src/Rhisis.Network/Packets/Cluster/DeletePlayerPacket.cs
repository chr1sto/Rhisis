using Sylver.Network.Data;

namespace Rhisis.Network.Packets.Cluster
{
    public class DeletePlayerPacket : IPacketDeserializer
    {
        public string Username { get; private set; }

        public string Password { get; private set; }

        public string PasswordConfirmation { get; private set; }

        public int CharacterId { get; private set; }

        public int AuthenticationKey { get; private set; }

        public void Deserialize(INetPacketStream packet)
        {
            this.Username = packet.Read<string>();
            this.Password = packet.Read<string>();
            this.PasswordConfirmation = packet.Read<string>();
            this.CharacterId = packet.Read<int>();
            this.AuthenticationKey = packet.Read<int>();
        }
    }
}
