using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class DialogPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the dialog owner object id.
        /// </summary>
        public uint ObjectId { get; set; }

        /// <summary>
        /// Gets the dialog key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets the first parameter.
        /// </summary>
        /// <remarks>
        /// Figure out what this value is.
        /// </remarks>
        public int Param { get; set; }

        /// <summary>
        /// Gets the dialog quest id.
        /// </summary>
        public int QuestId { get; set; }

        /// <summary>
        /// Gets the quest mover id.
        /// </summary>
        public uint MoverId { get; set; }

        /// <summary>
        /// Gets the quest player id.
        /// </summary>
        public uint PlayerId { get; set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            ObjectId = packet.Read<uint>();
            Key = packet.Read<string>();
            Param = packet.Read<int>();
            QuestId = packet.Read<int>();
            MoverId = packet.Read<uint>();
            PlayerId = packet.Read<uint>();
        }
    }
}
