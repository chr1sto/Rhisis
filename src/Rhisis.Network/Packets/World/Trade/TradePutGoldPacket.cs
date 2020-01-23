using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Trade
{
    public class TradePutGoldPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the amount of gold the player has placed in the trade.
        /// </summary>
        public int Gold { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            Gold = packet.Read<int>();
        }
    }
}
