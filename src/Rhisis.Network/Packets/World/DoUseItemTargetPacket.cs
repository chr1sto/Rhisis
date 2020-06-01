using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class DoUseItemTargetPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the material.
        /// </summary>
        public uint Material { get; private set; }

        /// <summary>
        /// Gets the target.
        /// </summary>
        public uint Target { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Material = packet.Read<uint>();
            Target = packet.Read<uint>();
        }
    }
}