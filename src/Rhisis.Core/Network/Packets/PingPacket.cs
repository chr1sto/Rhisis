using Ether.Network.Packets;
using System;

namespace Rhisis.Core.Network.Packets
{
    public struct PingPacket : IEquatable<PingPacket>
    {
        public int Time { get; }

        public PingPacket(INetPacketStream packet)
        {
            this.Time = packet.Read<int>();
        }

        public bool Equals(PingPacket other)
        {
            return this.Time == other.Time;
        }
    }
}
