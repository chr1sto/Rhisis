using Ether.Network.Packets;
using System;

namespace Rhisis.Core.Network
{
    public class FlyffPacketProcessor : IPacketProcessor
    {
        public int HeaderSize => 13;
        public bool IncludeHeader => false;
        public int GetMessageLength(byte[] buffer)
        {
            if (buffer[0] == FFPacket.Header)
            {
                var packetDataLength = new byte[4];

                Buffer.BlockCopy(buffer, 5, packetDataLength, 0, packetDataLength.Length);

                return BitConverter.ToInt32(packetDataLength, 0);
            }

            return 0;
        }

        public INetPacketStream CreatePacket(byte[] buffer) => new FFPacket(buffer);
    }
}
