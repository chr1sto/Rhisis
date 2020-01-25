using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class ExperiencePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the experience amount.
        /// </summary>
        public long Experience { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Experience = packet.Read<long>();
        }
    }
}