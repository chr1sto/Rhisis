using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Duel
{
    public class DuelRequestPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the source player id.
        /// </summary>
        public uint SourcePlayerId { get; private set; }

        /// <summary>
        /// Gets the destination player id.
        /// </summary>
        public uint DestinationPlayerId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            SourcePlayerId = packet.Read<uint>();
            DestinationPlayerId = packet.Read<uint>();
        }
    }
}