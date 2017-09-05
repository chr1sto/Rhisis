using Ether.Network.Packets;
using System;

namespace Rhisis.Core.Network.Packets.Cluster
{
    public struct GetPlayerListPacket : IEquatable<GetPlayerListPacket>
    {
        public string BuildVersion { get; private set; }

        public int AuthenticationKey { get; private set; }
        
        public string Username { get; private set; }

        public string Password { get; private set; }

        public int ServerId { get; private set; }

        public GetPlayerListPacket(NetPacketBase packet)
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
