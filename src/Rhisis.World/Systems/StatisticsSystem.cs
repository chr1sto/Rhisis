using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Rhisis.Core.IO;
using Rhisis.Core.Network.Packets.World;
using Rhisis.World.Core.Systems;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using Rhisis.World.Handlers;
using Rhisis.World.Systems.Events.Statistics;

namespace Rhisis.World.Systems
{
    [System]
    public class StatisticsSystem : NotifiableSystemBase
    {

        /// <summary>
        /// Gets the <see cref="InventorySystem"/> match filter.
        /// </summary>
        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.Player;

        public StatisticsSystem(IContext context) :
            base(context)
        {
        }

        /// <summary>
        /// Executes the <see cref="StatisticsSystem"/> logic.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        public override void Execute(IEntity entity, EventArgs e)
        {
            if (!(e is StatisticsEventArgs statisticsEvent))
                return;

            var playerEntity = entity as IPlayerEntity;

            Logger.Debug("Execute statistics action: {0}", statisticsEvent.ActionType.ToString());

            switch (statisticsEvent.ActionType)
            {
                case StatisticsActionType.ModifyStatus:
                    this.ModifyStatus(playerEntity, statisticsEvent.Arguments);
                    break;
                case StatisticsActionType.Unknown:
                    // Nothing to do.
                    break;
                default:
                    Logger.Warning("Unknown statistics action type: {0} for player {1} ", statisticsEvent.ActionType.ToString(), entity.ObjectComponent.Name);
                    break;
            }

            WorldPacketFactory.SendUpdateState(playerEntity);
        }

        private void ModifyStatus(IPlayerEntity player, object[] args)
        {
            Logger.Debug("Modify sttus");

            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Length < 0)
                throw new ArgumentException("Statistics event arguments cannot be empty.", nameof(args));

            var msPacket = args[0] as ModifyStatusPacket;
            var total = msPacket.StrenghtCount + msPacket.StaminaCount + msPacket.DexterityCount +
                        msPacket.IntelligenceCount;

            var statsPoints = player.StatisticsComponent.StatPoints;

            if (statsPoints <= 0)
                return;

            if (total <= 0 || total > statsPoints)
                return;

            if (msPacket.StrenghtCount < 0 || msPacket.StaminaCount < 0 || msPacket.DexterityCount < 0 ||
                msPacket.IntelligenceCount < 0)
                return;

            if (msPacket.StrenghtCount > statsPoints || msPacket.StaminaCount > statsPoints ||
                msPacket.DexterityCount > statsPoints || msPacket.IntelligenceCount > statsPoints)
                return;
            
            player.StatisticsComponent.Strenght += msPacket.StrenghtCount;
            player.StatisticsComponent.Stamina += msPacket.StaminaCount;
            player.StatisticsComponent.Dexterity += msPacket.DexterityCount;
            player.StatisticsComponent.Intelligence += msPacket.IntelligenceCount;
            player.StatisticsComponent.StatPoints -= total;
        }
    }
}
