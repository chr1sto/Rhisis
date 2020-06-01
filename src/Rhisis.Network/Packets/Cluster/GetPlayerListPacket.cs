using Sylver.Network.Data;

namespace Rhisis.Network.Packets.Cluster
{
    public class GetPlayerListPacket : IPacketDeserializer
    {
        public string BuildVersion { get; private set; }

        public int AuthenticationKey { get; private set; }
        
        public string Username { get; private set; }

        public string Password { get; private set; }

        public int ServerId { get; private set; }

        public void Deserialize(INetPacketStream packet)
        {
            BuildVersion = packet.Read<string>();
            AuthenticationKey = packet.Read<int>();
            Username = packet.Read<string>();
            Password = packet.Read<string>();
            ServerId = packet.Read<int>();
        }
    }
}
