using Sylver.Network.Data;

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
            Header = packet.Read<byte>();
            HashLength = packet.Read<int>();
            Length = packet.Read<int>();
            HashData = packet.Read<int>();
        }
    }
}
