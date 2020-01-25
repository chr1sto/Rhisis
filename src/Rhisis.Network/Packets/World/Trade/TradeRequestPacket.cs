using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Trade
{
    public sealed class TradeRequestPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the target object id for trade.
        /// </summary>
        public uint TargetId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            TargetId = packet.Read<uint>();
        }
    }
}
