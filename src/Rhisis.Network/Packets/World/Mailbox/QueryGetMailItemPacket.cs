using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Mailbox
{
    public class QueryGetMailItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the id of the mail
        /// </summary>
        public int MailId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            MailId = packet.Read<int>();
        }
    }
}
