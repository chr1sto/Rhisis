using System;
using System.Collections.Generic;
using System.Text;
using Ether.Network.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Network.Packets.World;
using Rhisis.World.Systems;
using Rhisis.World.Systems.Trade;

namespace Rhisis.World.Handlers
{
    internal static class TradeHandler
    {
        [PacketHandler(PacketType.CONFIRMTRADE)]
        public static void OnTradeRequest(WorldClient client, INetPacketStream packet)
        {
            var tradePacket = new TradeRequestPacket(packet);
            var tradeEvent = new TradeEventArgs(TradeActionType.TradeRequest, tradePacket.Target);

            client.Player.Context.NotifySystem<TradeSystem>(client.Player, tradeEvent);
        }

        [PacketHandler(PacketType.TRADE)]
        public static void OnTrade(WorldClient client, INetPacketStream packet)
        {
            var tradePacket = new TradeRequestPacket(packet);
            var tradeEvent = new TradeEventArgs(TradeActionType.Trade, tradePacket.Target);

            client.Player.Context.NotifySystem<TradeSystem>(client.Player, tradeEvent);
        }
    }
}
