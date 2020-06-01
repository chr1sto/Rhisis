using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Bank
{
    public class PutGoldBankPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the slot.
        /// </summary>
        public byte Slot { get; private set; }

        /// <summary>
        /// Gets the amount of gold.
        /// </summary>
        public uint Gold { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Slot = packet.Read<byte>();
            Gold = packet.Read<uint>();
        }
    }
}