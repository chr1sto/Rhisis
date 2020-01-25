using Sylver.Network.Data;
using Rhisis.Core.Structures;

namespace Rhisis.Network.Packets.World
{
    public class DropGoldPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the amount of gold.
        /// </summary>
        public uint Gold { get; private set; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Gold = packet.Read<uint>();
            Position = new Vector3(packet.Read<float>(), packet.Read<float>(), packet.Read<float>());
        }
    }
}