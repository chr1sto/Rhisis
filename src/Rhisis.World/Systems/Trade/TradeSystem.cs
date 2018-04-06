using Rhisis.Core.IO;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using System;
using System.Linq.Expressions;
using Rhisis.World.Core.Systems;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;

namespace Rhisis.World.Systems.Trade
{
    [System]
    internal sealed class TradeSystem : NotifiableSystemBase
    {
        public static int MaxTrade = 25;

        /// <summary>
        /// Gets the <see cref="TradeSystem"/> match filter.
        /// </summary>
        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.Player;

        public TradeSystem(IContext context) :
            base(context)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Executes the <see cref="T:Rhisis.World.Systems.Trade.TradeSystem" /> logic.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            if (!e.CheckArguments() || !(entity is IPlayerEntity playerEntity))
            {
                return;
            }

            switch (e)
            {
                case TradeRequestEventArgs tradeRequestEventArgs:
                    this.TradeRequest(playerEntity, tradeRequestEventArgs);
                    break;
                case TradeBeginEventArgs tradeBeginEventArgs:
                    this.Trade(playerEntity, tradeBeginEventArgs);
                    break;
                case TradePutEventArgs tradePutEventArgs:
                    this.PutItem(playerEntity, tradePutEventArgs);
                    break;
                case TradePutGoldEventArgs tradePutGoldEventArgs:
                    this.PutGold(playerEntity, tradePutGoldEventArgs);
                    break;
                default:
                    Logger.Warning("Unknown trade action type: {0} for player {1} ",
                        e.GetType(), entity.Object.Name);
                    break;
            }
        }
        
        private void TradeRequest(IPlayerEntity player, TradeRequestEventArgs e)
        {
            Logger.Debug("Trade request");

            if (e.TargetId == player.Id)
            {
                throw new Exception($"Can't start a Trade with ourselve ({e.TargetId})");
            }

            if (!(player.Context.FindEntity<IPlayerEntity>(e.TargetId) is IPlayerEntity target))
            {
                throw new Exception($"Can't find entity of id {e.TargetId}");
            }

            if (player.Trade.TargetId != 0 || target.Trade.TargetId != 0)
            {
                throw new Exception($"Can't start a Trade when one is already in progress ({player.Trade.TargetId})");
            }

            WorldPacketFactory.SendTradeRequest(target, player.Id);
        }
        private void Trade(IPlayerEntity player, TradeBeginEventArgs e)
        {
            if (e.TargetId == player.Id)
            {
                throw new Exception($"Can't start a Trade with ourselve ({e.TargetId})");
            }

            if (!(player.Context.FindEntity<IPlayerEntity>(e.TargetId) is IPlayerEntity target))
            {
                throw new Exception($"Can't find entity of id {e.TargetId}");
            }

            if (player.Trade.TargetId != 0 || target.Trade.TargetId != 0)
            {
                throw new Exception($"Can't start a Trade when one is already in progress ({player.Trade.TargetId})");
            }

            player.Trade.TargetId = target.Id;
            target.Trade.TargetId = player.Id;

            WorldPacketFactory.SendTrade(player, target, player.Id);
            WorldPacketFactory.SendTrade(target, player, player.Id);
        }

        private void PutItem(IPlayerEntity player, TradePutEventArgs e)
        {
            Logger.Debug("PutItem");

            if (player.Trade.TargetId == 0)
            {
                throw new Exception($"No trade target {player.Object.Name}");
            }

            if (!(player.Context.FindEntity<IPlayerEntity>(player.Trade.TargetId) is IPlayerEntity target))
            {
                throw new Exception($"Can't find entity of id {player.Trade.TargetId}");
            }

            if (player.Trade.State != TradeComponent.TradeState.Item || target.Trade.State != TradeComponent.TradeState.Item)
            {
                throw new Exception($"Not the right trade state {player.Trade.TargetId}");
            }

            var item = player.Inventory.GetItem(e.ItemId);
            if (item == null)
            {
                throw new NullReferenceException($"TradeSystem: Cannot find item with unique id: {e.ItemId}");
            }

            if (e.Count > item.Quantity)
            {
                throw new ArgumentException($"TradeSystem: More quantity than available for: {e.ItemId}");
            }

            var slotItem = player.Trade.Items.GetItemBySlot(e.Slot);
            if (slotItem != null && slotItem.Id != -1)
            {
                return;
            }

            player.Trade.Items.Items[e.Slot] = item;
            WorldPacketFactory.SendTradePut(player, player.Id, e.Slot, e.ItemType, e.ItemId, e.Count);
            WorldPacketFactory.SendTradePut(target, player.Id, e.Slot, e.ItemType, e.ItemId, e.Count);
        }

        private void PutGold(IPlayerEntity player, TradePutGoldEventArgs e)
        {
            Logger.Debug("PutGold");

            if (player.Trade.TargetId == 0)
            {
                throw new Exception($"No trade target {player.Object.Name}");
            }

            if (!(player.Context.FindEntity<IPlayerEntity>(player.Trade.TargetId) is IPlayerEntity target))
            {
                throw new Exception($"Can't find entity of id {player.Trade.TargetId}");
            }

            if (player.Trade.State != TradeComponent.TradeState.Item || target.Trade.State != TradeComponent.TradeState.Item)
            {
                throw new Exception($"Not the right trade state {player.Trade.TargetId}");
            }

            player.PlayerData.Gold -= (int)e.Gold;
            //TODO: Updating golds clientside

            player.Trade.Gold += e.Gold;

            WorldPacketFactory.SendTradePutGold(player, player.Id, player.Trade.Gold);
            WorldPacketFactory.SendTradePutGold(target, player.Id, player.Trade.Gold);
        }
    }
}
