using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class ScriptRemoveQuestPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the gold.
        /// </summary>
        public int QuestId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            QuestId = packet.Read<int>();
        }
    }
}