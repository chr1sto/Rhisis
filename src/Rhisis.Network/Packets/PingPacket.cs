using Sylver.Network.Data;
using System;

namespace Rhisis.Network.Packets
{
    public class PingPacket : IPacketDeserializer
    {
        public virtual int Time { get; private set; }

        public virtual bool IsTimeOut { get; private set; }

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
