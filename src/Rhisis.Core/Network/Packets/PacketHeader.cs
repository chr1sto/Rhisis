namespace Rhisis.Core.Network.Packets
{
    public class PacketHeader
    {
        public byte Header { get; private set; }

        public int HashLength { get; private set; }

        public int Length { get; private set; }

        public int HashData { get; private set; }

        public byte[] Data { get; private set; }

        public PacketHeader(FFPacket packet)
        {
            this.Header = packet.Read<byte>();
            this.HashLength = packet.Read<int>();
            this.Length = packet.Read<int>();
            this.HashData = packet.Read<int>();

            long streamPosition = packet.Position;
            this.Data = packet.ReadBytes(this.Length);
            packet.Position = streamPosition;
        }
    }
}
