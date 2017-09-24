using Ether.Network.Packets;
using System;

namespace Rhisis.Core.Network.Packets.Cluster
{
    public struct DeletePlayerPacket : IEquatable<DeletePlayerPacket>
    {
        public string Username { get; private set; }

        public string Password { get; private set; }

        public string PasswordConfirmation { get; private set; }

        public int CharacterId { get; private set; }

        public int AuthenticationKey { get; private set; }

        public DeletePlayerPacket(NetPacketBase packet)
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
