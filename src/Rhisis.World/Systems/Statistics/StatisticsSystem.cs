using System;
using System.Linq.Expressions;
using Rhisis.Core.IO;
using Rhisis.Core.Network.Packets.World;
using Rhisis.World.Core.Systems;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;

namespace Rhisis.World.Systems.Statistics
{
    [System]
    public class StatisticsSystem : NotifiableSystemBase
    {
        /// <inheritdoc />
        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.Player;

        /// <summary>
        /// Creates a new <see cref="StatisticsSystem"/> instance.
        /// </summary>
        /// <param name="context"></param>
        public StatisticsSystem(IContext context) :
            base(context)
        {
        }

        /// <inheritdoc />
        public override void Execute(IEntity entity, SystemEventArgs e)
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
                    Logger.Warning("Unknown statistics action type: {0} for player {1} ",
                        statisticsEvent.ActionType.ToString(), entity.Object.Name);
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
            if (!(args[0] is ModifyStatusPacket msPacket))
                throw new ArgumentException("Statistics event arguments can only be a ModifyStatusPacket.",
                    nameof(args));

            var total = msPacket.StrenghtCount + msPacket.StaminaCount + msPacket.DexterityCount +
                        msPacket.IntelligenceCount;

            var statsPoints = player.Statistics.StatPoints;
            if (statsPoints <= 0 || total > statsPoints)
            {
                Logger.Error("No statspoints available, but trying to upgrade {0}.", player.Object.Name);
                return;
            }

            if (msPacket.StrenghtCount > statsPoints || msPacket.StaminaCount > statsPoints ||
                msPacket.DexterityCount > statsPoints || msPacket.IntelligenceCount > statsPoints || total <= 0 ||
                total > ushort.MaxValue)
            {
                Logger.Error("Invalid upgrade request due to bad total calculation (trying to dupe) {0}.",
                    player.Object.Name);
                return;
            }

            player.Statistics.Strenght += msPacket.StrenghtCount;
            player.Statistics.Stamina += msPacket.StaminaCount;
            player.Statistics.Dexterity += msPacket.DexterityCount;
            player.Statistics.Intelligence += msPacket.IntelligenceCount;
            player.Statistics.StatPoints -= (ushort) total;
        }
    }
}
