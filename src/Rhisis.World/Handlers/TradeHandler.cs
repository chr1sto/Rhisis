using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.World.Trade;
using Rhisis.World.Systems.Trade;

namespace Rhisis.World.Handlers
{
    internal static class TradeHandler
    {
        [PacketHandler(PacketType.CONFIRMTRADE)]
        public static void OnTradeRequest(WorldClient client, INetPacketStream packet)
        {
            var tradePacket = new TradeRequestPacket(packet);
            var tradeEvent = new TradeRequestEventArgs(tradePacket.Target);

            client.Player.Context.NotifySystem<TradeSystem>(client.Player, tradeEvent);
        }

        [PacketHandler(PacketType.TRADE)]
        public static void OnTrade(WorldClient client, INetPacketStream packet)
        {
            var tradePacket = new TradeRequestPacket(packet);
            var tradeEvent = new TradeBeginEventArgs(tradePacket.Target);

            client.Player.Context.NotifySystem<TradeSystem>(client.Player, tradeEvent);
        }

        [PacketHandler(PacketType.TRADEPUT)]
        public static void OnTradePut(WorldClient client, INetPacketStream packet)
        {
            var tradePacket = new TradePutPacket(packet);
            var tradeEvent = new TradePutEventArgs(tradePacket.Position, tradePacket.ItemType, tradePacket.ItemId,
                tradePacket.Count);

            client.Player.Context.NotifySystem<TradeSystem>(client.Player, tradeEvent);
        }

        [PacketHandler(PacketType.TRADEPUTGOLD)]
        public static void OnTradePutGold(WorldClient client, INetPacketStream packet)
        {
            var tradePacket = new TradePutGoldPacket(packet);
            var tradeEvent = new TradePutGoldEventArgs(tradePacket.Gold);

            client.Player.Context.NotifySystem<TradeSystem>(client.Player, tradeEvent);
        }
    }
}
