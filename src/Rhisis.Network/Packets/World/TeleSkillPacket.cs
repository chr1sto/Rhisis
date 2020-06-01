using Sylver.Network.Data;
using Rhisis.Core.Structures;

namespace Rhisis.Network.Packets.World
{
    public class TeleSkillPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the position
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Position = new Vector3(packet.Read<float>(), packet.Read<float>(), packet.Read<float>());
        }
    }
}