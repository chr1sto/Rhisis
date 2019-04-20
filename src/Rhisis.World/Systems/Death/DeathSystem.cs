using Microsoft.Extensions.Logging;
using Rhisis.Core.Common.Formulas;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Loaders;
using Rhisis.World.Game.Maps;
using Rhisis.World.Game.Maps.Regions;
using Rhisis.World.Packets;

namespace Rhisis.World.Systems.Death
{
    [System(SystemType.Notifiable)]
    public sealed class DeathSystem : ISystem
    {
        private const float RecoveryRate = 0.2f;

        private readonly ILogger<DeathSystem> _logger = DependencyContainer.Instance.Resolve<ILogger<DeathSystem>>();
        private readonly MapLoader _mapLoader = DependencyContainer.Instance.Resolve<MapLoader>();

        public WorldEntityType Type => WorldEntityType.Player;

        public void Execute(IEntity entity, SystemEventArgs args)
        {
            if (!(entity is IPlayerEntity player))
            {
                this._logger.LogError($"Cannot execute DeathSystem. {entity.Object.Name} is not a player.");
                return;
            }

            IMapRevivalRegion revivalRegion = player.Object.CurrentMap.GetNearRevivalRegion(player.Object.Position);

            if (revivalRegion == null)
            {
                this._logger.LogError($"Cannot find any revival region for map '{player.Object.CurrentMap.Name}'.");
                return;
            }

            var jobData = player.PlayerData.JobData;
            var playerStats = player.Statistics;

            player.Health.Hp = (int)(HealthFormulas.GetMaxOriginHp(player.Object.Level, playerStats.Stamina, jobData.MaxHpFactor) * RecoveryRate);
            player.Health.Mp = (int)(HealthFormulas.GetMaxOriginMp(player.Object.Level, playerStats.Intelligence, jobData.MaxMpFactor, true) * RecoveryRate);
            player.Health.Fp = (int)(HealthFormulas.GetMaxOriginFp(player.Object.Level, playerStats.Stamina, playerStats.Dexterity, playerStats.Strength, jobData.MaxFpFactor, true) * RecoveryRate);

            WorldPacketFactory.SendUpdateAttributes(player, DefineAttributes.HP, player.Health.Hp);
            WorldPacketFactory.SendUpdateAttributes(player, DefineAttributes.MP, player.Health.Mp);
            WorldPacketFactory.SendUpdateAttributes(player, DefineAttributes.FP, player.Health.Fp);

            bool shouldReplace = false;
            if (revivalRegion.MapId != player.Object.MapId)
            {
                IMapInstance revivalMap = this._mapLoader.GetMapById(revivalRegion.MapId);

                if (revivalMap == null)
                {
                    this._logger.LogError($"Cannot find revival map with id '{revivalRegion.MapId}'.");
                    player.Connection.Server.DisconnectClient(player.Connection.Id);
                    return;
                }

                IMapLayer revivalMapLayer = revivalMap.GetDefaultMapLayer();
                player.SwitchContext(revivalMapLayer);
                player.Object.Spawned = false;
                player.Object.MapId = revivalMap.Id;
                player.Object.LayerId = revivalMapLayer.Id;

                shouldReplace = true;
                revivalRegion = revivalMap.GetRevivalRegion(revivalRegion.Key);
            }

            player.Object.Position = revivalRegion.RevivalPosition.Clone();
            player.MovableComponent.DestinationPosition = revivalRegion.RevivalPosition.Clone();

            if (shouldReplace)
            {
                WorldPacketFactory.SendReplaceObject(player);
                WorldPacketFactory.SendPlayerSpawn(player);
                player.Object.Spawned = true;
            }

            WorldPacketFactory.SendMotion(player, ObjectMessageType.OBJMSG_ACC_STOP | ObjectMessageType.OBJMSG_STOP_TURN | ObjectMessageType.OBJMSG_STAND);
            WorldPacketFactory.SendPlayerRevival(player);
            WorldPacketFactory.SendMoverPosition(player);
        }
    }
}
