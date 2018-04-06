using Ether.Network.Packets;

namespace Rhisis.Core.Network.Packets.World.Trade
{
    public class TradePutGoldPacket
    {
        public uint Gold { get; private set; }

        public TradePutGoldPacket(INetPacketStream packet)
        {
            Gold = packet.Read<uint>();
        }
    }
}
