using Sylver.Network.Data;

namespace Rhisis.Network.Packets.Cluster
{
    public class QueryTickCountPacket : IPacketDeserializer
    {
        public uint Time { get; private set; }

        public void Deserialize(INetPacketStream packet)
        {
            Time = packet.Read<uint>();
        }
    }
}
