using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Mailbox
{
    public class QueryPostMailPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the slot of the item.
        /// </summary>
        public byte ItemSlot { get; private set; }

        /// <summary>
        /// Gets the quantity of the item.
        /// </summary>
        public short ItemQuantity { get; private set; }

        /// <summary>
        /// Gets the receiver's name.
        /// </summary>
        public string Receiver { get; private set; }

        /// <summary>
        /// Gets the amount of gold.
        /// </summary>
        public int Gold { get; private set; }

        /// <summary>
        /// Gets the title of the mail.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the text of the mail.
        /// </summary>
        public string Text { get; private set; }


        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            ItemSlot = packet.Read<byte>();
            ItemQuantity = packet.Read<short>();
            Receiver = packet.Read<string>();
            Gold = packet.Read<int>();
            Title = packet.Read<string>();
            Text = packet.Read<string>();
        }
    }
}
