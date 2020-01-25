using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class OpenShopWindowPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the selected object id.
        /// </summary>
        public uint ObjectId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            ObjectId = packet.Read<uint>();
        }
    }
}
