using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.Cluster
{
    public class GetPlayerListPacket : IEquatable<GetPlayerListPacket>, IPacketDeserializer
    {
        public string BuildVersion { get; private set; }

        public int AuthenticationKey { get; private set; }
        
        public string Username { get; private set; }

        public string Password { get; private set; }

        public int ServerId { get; private set; }

        public bool Equals(GetPlayerListPacket other)
        {
            return this.BuildVersion == other.BuildVersion
                && this.AuthenticationKey == other.AuthenticationKey
                && this.Username == other.Username
                && this.Password == other.Password
                && this.ServerId == other.ServerId;
        }

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
