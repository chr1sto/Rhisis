using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class ScriptAddGoldPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the gold.
        /// </summary>
        public int Gold { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Gold = packet.Read<int>();
        }
    }
}