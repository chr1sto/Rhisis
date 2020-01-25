using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class FocusObjectPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the object id.
        /// </summary>
        public uint ObjectId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            ObjectId = packet.Read<uint>();
        }
    }
}