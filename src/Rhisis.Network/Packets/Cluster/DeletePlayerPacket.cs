using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.Cluster
{
    public class DeletePlayerPacket : IEquatable<DeletePlayerPacket>, IPacketDeserializer
    {
        public string Username { get; private set; }

        public string Password { get; private set; }

        public string PasswordConfirmation { get; private set; }

        public int CharacterId { get; private set; }

        public int AuthenticationKey { get; private set; }

        public bool Equals(DeletePlayerPacket other)
        {
            return this.Username == other.Username &&
                this.Password == other.Password &&
                this.PasswordConfirmation == other.PasswordConfirmation &&
                this.CharacterId == other.CharacterId &&
                this.AuthenticationKey == other.AuthenticationKey;
        }

        public void Deserialize(INetPacketStream packet)
        {
            this.Username = packet.Read<string>();
            this.Password = packet.Read<string>();
            this.PasswordConfirmation = packet.Read<string>();
            this.CharacterId = packet.Read<int>();
            this.AuthenticationKey = packet.Read<int>();
        }
    }
}
