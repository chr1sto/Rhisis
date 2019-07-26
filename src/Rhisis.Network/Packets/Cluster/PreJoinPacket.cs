using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.Cluster
{
    public class PreJoinPacket : IEquatable<PreJoinPacket>, IPacketDeserializer
    {
        public string Username { get; private set; }

        public int CharacterId { get; private set; }

        public string CharacterName { get; private set; }

        public int BankCode { get; private set; }

        public bool Equals(PreJoinPacket other)
        {
            return this.Username.Equals(other.Username) &&
                this.CharacterId == other.CharacterId &&
                this.CharacterName.Equals(other.CharacterName) &&
                this.BankCode == other.BankCode;
        }

        public void Deserialize(INetPacketStream packet)
        {
            this.Username = packet.Read<string>();
            this.CharacterId = packet.Read<int>();
            this.CharacterName = packet.Read<string>();
            this.BankCode = packet.Read<int>();
        }
    }
}
