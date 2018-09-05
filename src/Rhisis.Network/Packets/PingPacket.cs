using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets
{
    public struct PingPacket : IEquatable<PingPacket>
    {
        public int Time { get; }

        public bool IsTimeOut { get; }

        public PingPacket(INetPacketStream packet)
        {
            try
            {
                this.Time = packet.Read<int>();
                this.IsTimeOut = false;
            }
            catch (Exception)
            {
                this.Time = 0;
                this.IsTimeOut = true;
            }
        }

        public bool Equals(PingPacket other) => this.Time == other.Time && this.IsTimeOut == other.IsTimeOut;
    }
}
