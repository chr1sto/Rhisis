using Ether.Network.Packets;
using System;

namespace Rhisis.Network.Packets
{
    public class PingPacket : IEquatable<PingPacket>, IPacketDeserializer
    {
        public virtual int Time { get; private set; }

        public virtual bool IsTimeOut { get; private set; }

        public bool Equals(PingPacket other) => this.Time == other.Time && this.IsTimeOut == other.IsTimeOut;

        public void Deserialize(INetPacketStream packet)
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
    }
}
