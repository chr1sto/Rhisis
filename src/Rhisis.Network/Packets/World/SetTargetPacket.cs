using Sylver.Network.Data;
using Rhisis.Core.Common;

namespace Rhisis.Network.Packets.World
{
    public class SetTargetPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the target id.
        /// </summary>
        public uint TargetId { get; private set; }

        /// <summary>
        /// Gets a value indicating whether target should be cleared or not.
        /// </summary>
        public TargetModeType TargetMode { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            TargetId = packet.Read<uint>();
            TargetMode = (TargetModeType)packet.Read<byte>();
        }
    }
}