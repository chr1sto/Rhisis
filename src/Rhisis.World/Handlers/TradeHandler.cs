using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World.Trade;
using Rhisis.World.Client;
using Rhisis.World.Systems.Trade;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.World.Handlers
{
    [Handler]
    public class TradeHandler
    {
        private readonly ITradeSystem _tradeSystem;

        public TradeHandler(ITradeSystem tradeSystem)
        {
            _tradeSystem = tradeSystem;
        }

        [HandlerAction(PacketType.CONFIRMTRADE)]
        public void OnTradeRequest(IWorldClient client, TradeRequestPacket packet)
        {
            _tradeSystem.RequestTrade(client.Player, packet.TargetId);
        }

        [HandlerAction(PacketType.CONFIRMTRADECANCEL)]
        public void OnTradeRequestCancel(IWorldClient client, TradeRequestPacket packet)
        {
            _tradeSystem.DeclineTradeRequest(client.Player, packet.TargetId);
        }

        [HandlerAction(PacketType.TRADE)]
        public void OnTrade(IWorldClient client, TradeRequestPacket packet)
        {
            _tradeSystem.StartTrade(client.Player, packet.TargetId);
        }

        [HandlerAction(PacketType.TRADEPUT)]
        public void OnTradePut(IWorldClient client, TradePutPacket packet)
        {
            _tradeSystem.PutItem(client.Player, packet.ItemUniqueId, packet.Count, packet.ItemType, packet.Position);
        }

        [HandlerAction(PacketType.TRADEPUTGOLD)]
        public void OnTradePutGold(IWorldClient client, TradePutGoldPacket packet)
        {
            _tradeSystem.PutGold(client.Player, packet.Gold);
        }

        [HandlerAction(PacketType.TRADECANCEL)]
        public void OnTradeCancel(IWorldClient client, TradeCancelPacket packet)
        {
            _tradeSystem.CancelTrade(client.Player, packet.Mode);
        }

        [HandlerAction(PacketType.TRADEOK)]
        public void OnTradeOk(IWorldClient client)
        {
            _tradeSystem.ConfirmTrade(client.Player);
        }

        [HandlerAction(PacketType.TRADECONFIRM)]
        public void OnTradeConfirm(IWorldClient client)
        {
            _tradeSystem.LastConfirmTrade(client.Player);
        }
    }
}
