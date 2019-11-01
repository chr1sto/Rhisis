using Sylver.Network.Data;
using System;

namespace Rhisis.Network
{
    public class FlyffPacketProcessor : IPacketProcessor
    {
        /// <summary>
        /// Gets the FlyFF packet header size.
        /// </summary>
        public int HeaderSize => 13;

        /// <inheritdoc />
        public bool IncludeHeader => false;

        /// <inheritdoc />
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

        /// <inheritdoc />
        public INetPacketStream CreatePacket(byte[] buffer) => new FFPacket(buffer);
    }
}
