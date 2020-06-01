using Sylver.Network.Data;

namespace Rhisis.Network.Packets.Login
{
    public class CloseConnectionPacket : IPacketDeserializer
    {
        public string Username { get; private set; }

        public string Password { get; private set; }

        public void Deserialize(INetPacketStream packet)
        {
            Username = packet.Read<string>();
            Password = packet.Read<string>();
        }
    }
}