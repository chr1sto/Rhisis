using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Trade
{
    public class TradePullPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the slot.
        /// </summary>
        public byte Slot { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Slot = packet.Read<byte>();
        }
    }
}