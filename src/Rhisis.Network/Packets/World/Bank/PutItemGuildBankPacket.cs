using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Bank
{
    public class PutItemGuildBankPacket : IPacketDeserializer
    {

        public byte Id { get; private set; }

        public uint ItemId { get; private set; }

        public byte Mode { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Id = packet.Read<byte>();
            ItemId = packet.Read<uint>();
            Mode = packet.Read<byte>();
        }
    }
}