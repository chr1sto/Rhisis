using Rhisis.Core.IO;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using System;
using System.Linq.Expressions;
using Rhisis.World.Core.Systems;
using Rhisis.World.Packets;

namespace Rhisis.World.Systems.Trade
{
    [System]
    internal sealed class TradeSystem : NotifiableSystemBase
    {
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
            if (!(e is TradeEventArgs tradeEvent))
                return;

            var playerEntity = entity as IPlayerEntity;

            Logger.Debug("Execute trade action: {0}", tradeEvent.ActionType.ToString());

            switch (tradeEvent.ActionType)
            {
                case TradeActionType.TradeRequest:
                    this.TradeRequest(playerEntity, tradeEvent.Arguments);
                    break;
                case TradeActionType.Trade:
                    this.Trade(playerEntity, tradeEvent.Arguments);
                    break;
                case TradeActionType.Unknown:
                    // Nothing to do.
                    break;
                default:
                    Logger.Warning("Unknown trade action type: {0} for player {1} ",
                        tradeEvent.ActionType.ToString(), entity.ObjectComponent.Name);
                    break;
            }
        }
        
        private void TradeRequest(IPlayerEntity player, object[] args)
        {
            Logger.Debug("Trade request");

            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Length < 0)
                throw new ArgumentException("Trade event arguments cannot be empty.", nameof(args));

            if (!(args[0] is int target))
                throw new ArgumentException("Trade request argument have to be an integer.", nameof(args));
            
            if (!(player.Context.FindEntity<IPlayerEntity>(target) is IPlayerEntity targetEntity))
            {
                Logger.Error($"Can't find entity of id {target}");
                return;
            }

            WorldPacketFactory.SendTradeRequest(targetEntity, player.Id);
        }

        private void Trade(IPlayerEntity player, object[] args)
        {
            Logger.Debug("Trade");

            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Length < 0)
                throw new ArgumentException("Trade event arguments cannot be empty.", nameof(args));

            if (!(args[0] is int target))
                throw new ArgumentException("Trade request argument have to be an integer.", nameof(args));

            if (!(player.Context.FindEntity<IPlayerEntity>(target) is IPlayerEntity targetEntity))
            {
                Logger.Error($"Can't find entity of id {target}");
                return;
            }

            player.Trade.TargetId = targetEntity.PlayerComponent.Id;
            targetEntity.Trade.TargetId = player.PlayerComponent.Id;

            WorldPacketFactory.SendTrade(player, targetEntity.Id, player.Id);
            WorldPacketFactory.SendTrade(targetEntity, player.Id, player.Id);
        }
    }
}
