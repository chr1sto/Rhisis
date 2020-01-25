using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class TagPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the target id.
        /// </summary>
        public uint TargetId { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            TargetId = packet.Read<uint>();
            Message = packet.Read<string>();
        }
    }
}