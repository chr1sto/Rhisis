using Ether.Network.Packets;
using System;

namespace Rhisis.Core.Network.Packets
{
    public struct PingPacket : IEquatable<PingPacket>
    {
        public int Time { get; private set; }

        public PingPacket(NetPacketBase packet)
        {
            this.Time = packet.Read<int>();
        }

        public bool Equals(PingPacket other)
        {
            return this.Time == other.Time;
        }
    }
}
