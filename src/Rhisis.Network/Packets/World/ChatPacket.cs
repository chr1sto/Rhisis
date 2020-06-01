using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class ChatPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the chat message.
        /// </summary>
        public string Message { get; private set; }
        
        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Message = packet.Read<string>();
        }
    }
}
