using Ether.Network.Packets;

namespace Rhisis.Network.Packets.Login
{
    public class CloseConnectionPacket : IPacketDeserializer
    {
        public string Username { get; private set; }

        public string Password { get; private set; }

        public void Deserialize(INetPacketStream packet)
        {
            this.Username = packet.Read<string>();
            this.Password = packet.Read<string>();
        }
    }
}