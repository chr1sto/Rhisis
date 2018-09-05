using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.Cluster
{
    public struct DeletePlayerPacket : IEquatable<DeletePlayerPacket>
    {
        public string Username { get; }

        public string Password { get; }

        public string PasswordConfirmation { get; }

        public int CharacterId { get; }

        public int AuthenticationKey { get; }

        public DeletePlayerPacket(INetPacketStream packet)
        {
            this.Username = packet.Read<string>();
            this.Password = packet.Read<string>();
            this.PasswordConfirmation = packet.Read<string>();
            this.CharacterId = packet.Read<int>();
            this.AuthenticationKey = packet.Read<int>();
        }

        public bool Equals(DeletePlayerPacket other)
        {
            return this.Username == other.Username &&
                this.Password == other.Password &&
                this.PasswordConfirmation == other.PasswordConfirmation &&
                this.CharacterId == other.CharacterId &&
                this.AuthenticationKey == other.AuthenticationKey;
        }
    }
}
