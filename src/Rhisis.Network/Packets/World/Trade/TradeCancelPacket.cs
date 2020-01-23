using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Trade
{
    public class TradeCancelPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the trade cancel mode.
        /// </summary>
        public int Mode { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Mode = packet.Read<int>();
        }
    }
}
