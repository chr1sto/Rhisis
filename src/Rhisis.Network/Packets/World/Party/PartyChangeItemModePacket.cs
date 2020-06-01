using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Party
{
    public class PartyChangeItemModePacket : IPacketDeserializer
    {

        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; private set; }

        /// <summary>
        /// Gets the item mode.
        /// </summary>
        public int ItemMode { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            PlayerId = packet.Read<uint>();
            ItemMode = packet.Read<int>();
        }
    }
}