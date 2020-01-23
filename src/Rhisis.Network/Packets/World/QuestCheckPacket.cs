using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class QuestCheckPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the quest id.
        /// </summary>
        public int QuestId { get; private set; }

        /// <summary>
        /// Gets the quest checked state.
        /// </summary>
        public bool Checked { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            QuestId = packet.Read<int>();
            Checked = packet.Read<bool>();
        }
    }
}