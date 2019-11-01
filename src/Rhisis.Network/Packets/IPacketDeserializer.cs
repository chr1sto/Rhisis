using Sylver.Network.Data;

namespace Rhisis.Network.Packets
{
    public interface IPacketDeserializer
    {
        void Deserialize(INetPacketStream packet);
    }
}
