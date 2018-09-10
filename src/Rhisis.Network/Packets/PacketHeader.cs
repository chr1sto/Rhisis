using Ether.Network.Packets;

namespace Rhisis.Network.Packets
{
    public class PacketHeader
    {
        public byte Header { get; }

        public int HashLength { get; }

        public int Length { get; }

        public int HashData { get; }

        public PacketHeader(INetPacketStream packet)
        {
            this.Header = packet.Read<byte>();
            this.HashLength = packet.Read<int>();
            this.Length = packet.Read<int>();
            this.HashData = packet.Read<int>();
        }
    }
}
