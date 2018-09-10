using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.Cluster
{
    public struct GetPlayerListPacket : IEquatable<GetPlayerListPacket>
    {
        public string BuildVersion { get; }

        public int AuthenticationKey { get; }
        
        public string Username { get; }

        public string Password { get; }

        public int ServerId { get; }

        public GetPlayerListPacket(INetPacketStream packet)
        {
            this.BuildVersion = packet.Read<string>();
            this.AuthenticationKey = packet.Read<int>();
            this.Username = packet.Read<string>();
            this.Password = packet.Read<string>();
            this.ServerId = packet.Read<int>();
        }

        public bool Equals(GetPlayerListPacket other)
        {
            return this.BuildVersion == other.BuildVersion
                && this.AuthenticationKey == other.AuthenticationKey
                && this.Username == other.Username
                && this.Password == other.Password
                && this.ServerId == other.ServerId;
        }
    }
}
