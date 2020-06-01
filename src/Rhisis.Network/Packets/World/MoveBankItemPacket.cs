using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class MoveBankItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the source index.
        /// </summary>
        public int SourceIndex { get; private set; }

        /// <summary>
        /// Gets the destination index.
        /// </summary>
        public int DestinationIndex { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            SourceIndex = packet.Read<int>();
            DestinationIndex = packet.Read<int>();
        }
    }
}