using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Party
{
    public class PartyChangeExperienceModePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; private set; }

        /// <summary>
        /// Gets the experience mode.
        /// </summary>
        public int ExperienceMode { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            PlayerId = packet.Read<uint>();
            ExperienceMode = packet.Read<int>();
        }
    }
}