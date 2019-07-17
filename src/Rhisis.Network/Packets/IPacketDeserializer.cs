using Ether.Network.Packets;

namespace Rhisis.Network.Packets
{
    public interface IPacketDeserializer
    {
        void Deserialize(INetPacketStream packet);
    }
}
