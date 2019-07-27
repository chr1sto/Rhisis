using Ether.Network.Packets;

namespace Rhisis.Network.Packets.Cluster
{
    public class QueryTickCountPacket : IPacketDeserializer
    {
        public uint Time { get; private set; }

        public void Deserialize(INetPacketStream packet)
        {
            this.Time = packet.Read<uint>();
        }
    }
}
