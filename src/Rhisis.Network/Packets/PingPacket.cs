using Sylver.Network.Data;
using System;

namespace Rhisis.Network.Packets
{
    /// <summary>
    /// Represents the ping packet.
    /// </summary>
    public class PingPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the ping packet time.
        /// </summary>
        public virtual int Time { get; private set; }

        /// <summary>
        /// Gets a value that indiciates that the ping packet has timeout.
        /// </summary>
        public virtual bool IsTimeOut { get; private set; }

        /// <inheritdoc />
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
