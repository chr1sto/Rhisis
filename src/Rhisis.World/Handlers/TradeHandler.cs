using Ether.Network.Packets;
using Rhisis.Network;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World.Trade;
using Rhisis.World.Systems.Trade;
using Rhisis.World.Systems.Trade.EventArgs;

namespace Rhisis.World.Handlers
{
    internal static class TradeHandler
    {
        [PacketHandler(PacketType.CONFIRMTRADE)]
        public static void OnTradeRequest(WorldClient client, INetPacketStream packet)
        {
            var tradePacket = new TradeRequestPacket(packet);
            var tradeEvent = new TradeRequestEventArgs(tradePacket.Target);

            client.Player.NotifySystem<TradeSystem>(tradeEvent);
        }

        [PacketHandler(PacketType.CONFIRMTRADECANCEL)]
        public static void OnTradeRequestCancel(WorldClient client, INetPacketStream packet)
        {
            var tradePacket = new TradeRequestPacket(packet);
            var tradeEvent = new TradeRequestCancelEventArgs(tradePacket.Target);

            client.Player.NotifySystem<TradeSystem>(tradeEvent);
        }

        [PacketHandler(PacketType.TRADE)]
        public static void OnTrade(WorldClient client, INetPacketStream packet)
        {
            var tradePacket = new TradeRequestPacket(packet);
            var tradeEvent = new TradeBeginEventArgs(tradePacket.Target);

            client.Player.NotifySystem<TradeSystem>(tradeEvent);
        }

        [PacketHandler(PacketType.TRADEPUT)]
        public static void OnTradePut(WorldClient client, INetPacketStream packet)
        {
            var tradePacket = new TradePutPacket(packet);
            var tradeEvent = new TradePutEventArgs(tradePacket.Position, tradePacket.ItemType, tradePacket.ItemId,
                tradePacket.Count);

            client.Player.NotifySystem<TradeSystem>(tradeEvent);
        }

        [PacketHandler(PacketType.TRADEPUTGOLD)]
        public static void OnTradePutGold(WorldClient client, INetPacketStream packet)
        {
            var tradePacket = new TradePutGoldPacket(packet);
            var tradeEvent = new TradePutGoldEventArgs(tradePacket.Gold);

            client.Player.NotifySystem<TradeSystem>(tradeEvent);
        }

        [PacketHandler(PacketType.TRADECANCEL)]
        public static void OnTradeCancel(WorldClient client, INetPacketStream packet)
        {
            var tradePacket = new TradeCancelPacket(packet);
            var tradeEvent = new TradeCancelEventArgs(tradePacket.Mode);

            client.Player.NotifySystem<TradeSystem>(tradeEvent);
        }

        [PacketHandler(PacketType.TRADEOK)]
        public static void OnTradeOk(WorldClient client, INetPacketStream packet)
        {
            client.Player.NotifySystem<TradeSystem>(new TradeOkEventArgs());
        }

        [PacketHandler(PacketType.TRADECONFIRM)]
        public static void OnTradeConfirm(WorldClient client, INetPacketStream packet)
        {
            client.Player.NotifySystem<TradeSystem>(new TradeConfirmEventArgs());
        }
    }
}
