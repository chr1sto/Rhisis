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
            Username = packet.Read<string>();
            Password = packet.Read<string>();
            PasswordConfirmation = packet.Read<string>();
            CharacterId = packet.Read<int>();
            AuthenticationKey = packet.Read<int>();
        }
    }
}
