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
                Time = packet.Read<int>();
                IsTimeOut = false;
            }
            catch (Exception)
            {
                Time = 0;
                IsTimeOut = true;
            }
        }
    }
}
