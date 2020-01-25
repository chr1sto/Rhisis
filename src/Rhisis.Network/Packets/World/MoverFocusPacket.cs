using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class MoverFocusPacket : IPacketDeserializer
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