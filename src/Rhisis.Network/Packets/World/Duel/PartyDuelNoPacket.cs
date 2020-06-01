using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Duel
{
    public class PartyDuelNoPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            PlayerId = packet.Read<uint>();
        }
    }
}