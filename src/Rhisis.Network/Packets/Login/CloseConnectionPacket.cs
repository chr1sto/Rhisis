using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.Login
{
    public struct CloseConnectionPacket : IEquatable<CloseConnectionPacket>
    {
        public string Username { get; }

        public string Password { get; }

        public CloseConnectionPacket(INetPacketStream packet)
        {
            this.Username = packet.Read<string>();
            this.Password = packet.Read<string>();
        }

        public bool Equals(CloseConnectionPacket other) =>
            this.Username.Equals(other.Username, StringComparison.OrdinalIgnoreCase) &&
            this.Password.Equals(other.Password, StringComparison.OrdinalIgnoreCase);
    }
}