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
        public void OnTradeRequest(IWorldServerClient serverClient, TradeRequestPacket packet)
        {
            _tradeSystem.RequestTrade(serverClient.Player, packet.TargetId);
        }

        [HandlerAction(PacketType.CONFIRMTRADECANCEL)]
        public void OnTradeRequestCancel(IWorldServerClient serverClient, TradeRequestPacket packet)
        {
            _tradeSystem.DeclineTradeRequest(serverClient.Player, packet.TargetId);
        }

        [HandlerAction(PacketType.TRADE)]
        public void OnTrade(IWorldServerClient serverClient, TradeRequestPacket packet)
        {
            _tradeSystem.StartTrade(serverClient.Player, packet.TargetId);
        }

        [HandlerAction(PacketType.TRADEPUT)]
        public void OnTradePut(IWorldServerClient serverClient, TradePutPacket packet)
        {
            _tradeSystem.PutItem(serverClient.Player, packet.ItemUniqueId, packet.Count, packet.ItemType, packet.Position);
        }

        [HandlerAction(PacketType.TRADEPUTGOLD)]
        public void OnTradePutGold(IWorldServerClient serverClient, TradePutGoldPacket packet)
        {
            _tradeSystem.PutGold(serverClient.Player, packet.Gold);
        }

        [HandlerAction(PacketType.TRADECANCEL)]
        public void OnTradeCancel(IWorldServerClient serverClient, TradeCancelPacket packet)
        {
            _tradeSystem.CancelTrade(serverClient.Player, packet.Mode);
        }

        [HandlerAction(PacketType.TRADEOK)]
        public void OnTradeOk(IWorldServerClient serverClient)
        {
            _tradeSystem.ConfirmTrade(serverClient.Player);
        }

        [HandlerAction(PacketType.TRADECONFIRM)]
        public void OnTradeConfirm(IWorldServerClient serverClient)
        {
            _tradeSystem.LastConfirmTrade(serverClient.Player);
        }
    }
}
