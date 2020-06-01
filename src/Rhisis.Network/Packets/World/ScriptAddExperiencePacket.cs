using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class ScriptAddExperiencePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public long Experience { get; private set; }

        /// <inheritdoc />
        public void Deserialize (INetPacketStream packet)
        {
            Experience = packet.Read<long>();
        }
    }
}