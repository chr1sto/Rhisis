using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.Cluster
{
    public class QueryTickCountPacket : IEquatable<QueryTickCountPacket>, IPacketDeserializer
    {
        public uint Time { get; private set; }

        public bool Equals(QueryTickCountPacket other) => this.Time == other.Time;

        public void Deserialize(INetPacketStream packet)
        {
            this.Time = packet.Read<uint>();
        }
    }
}
