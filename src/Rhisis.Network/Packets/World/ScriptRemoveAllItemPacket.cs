using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class ScriptRemoveAllItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ScriptRemoveAllItemPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public void Deserialize(INetPacketStream packet)
        {
            ItemId = packet.Read<uint>();
        }
    }
}