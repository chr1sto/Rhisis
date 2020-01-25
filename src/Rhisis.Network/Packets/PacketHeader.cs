using Sylver.Network.Data;

namespace Rhisis.Network.Packets
{
    public class PacketHeader : IPacketDeserializer
    {
        public byte Header { get; private set; }

        public int HashLength { get; private set; }

        public int Length { get; private set; }

        public int HashData { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Header = packet.Read<byte>();
            HashLength = packet.Read<int>();
            Length = packet.Read<int>();
            HashData = packet.Read<int>();
        }
    }
}
