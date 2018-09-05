using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.Cluster
{
    public struct PreJoinPacket : IEquatable<PreJoinPacket>
    {
        public string Username { get; }

        public int CharacterId { get; }

        public string CharacterName { get; }

        public int BankCode { get; }

        public PreJoinPacket(INetPacketStream packet)
        {
            this.Username = packet.Read<string>();
            this.CharacterId = packet.Read<int>();
            this.CharacterName = packet.Read<string>();
            this.BankCode = packet.Read<int>();
        }

        public bool Equals(PreJoinPacket other)
        {
            return this.Username.Equals(other.Username) &&
                this.CharacterId == other.CharacterId &&
                this.CharacterName.Equals(other.CharacterName) &&
                this.BankCode == other.BankCode;
        }
    }
}
