using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class GetClockPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the SetBaseOfClient.
        /// </summary>
        public byte SetBaseOfClient { get; private set; }

        /// <summary>
        /// Gets the client time
        /// </summary>
        public uint ClientTime { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            SetBaseOfClient = packet.Read<byte>();
            ClientTime = packet.Read<uint>();
        }
    }
}