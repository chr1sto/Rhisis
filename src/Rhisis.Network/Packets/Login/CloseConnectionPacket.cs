using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.Login
{
    public class CloseConnectionPacket : IEquatable<CloseConnectionPacket>, IPacketDeserializer
    {
        public string Username { get; private set; }

        public string Password { get; private set; }

        public bool Equals(CloseConnectionPacket other) =>
            this.Username.Equals(other.Username, StringComparison.OrdinalIgnoreCase) &&
            this.Password.Equals(other.Password, StringComparison.OrdinalIgnoreCase);

        public void Deserialize(INetPacketStream packet)
        {
            this.Username = packet.Read<string>();
            this.Password = packet.Read<string>();
        }
    }
}