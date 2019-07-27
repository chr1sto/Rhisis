using Ether.Network.Packets;

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
            this.BuildVersion = packet.Read<string>();
            this.AuthenticationKey = packet.Read<int>();
            this.Username = packet.Read<string>();
            this.Password = packet.Read<string>();
            this.ServerId = packet.Read<int>();
        }
    }
}
