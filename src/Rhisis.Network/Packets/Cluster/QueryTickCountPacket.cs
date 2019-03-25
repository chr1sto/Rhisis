using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets.Cluster
{
    public struct QueryTickCountPacket : IEquatable<QueryTickCountPacket>
    {
        public uint Time { get; }

        public QueryTickCountPacket(INetPacketStream packet)
        {
            this.Time = packet.Read<uint>();
        }

        public bool Equals(QueryTickCountPacket other)
        {
            return this.Time == other.Time;
        }
    }
}
