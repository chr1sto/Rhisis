using Microsoft.Extensions.Logging;
using Rhisis.Core.Common.Formulas;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.IO;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Entities;
using Rhisis.World.Systems.Recovery.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Systems.Recovery
{
    /// <summary>
    /// Game system that manages all recoveries. HP, MP, FP, etc...
    /// </summary>
    [System(SystemType.Notifiable)]
    public sealed class RecoverySystem : ISystem
    {
        /// <summary>
        /// Gets the number of seconds for next idle heal when the player is sitted.
        /// </summary>
        private const int NextIdleHealSit = 2;

        /// <summary>
        /// Gets the number of seconds for the idle heal when the player stands up.
        /// </summary>
        private const int NextIdleHealStand = 3;

        private readonly ILogger<RecoverySystem> _logger;

        /// <inheritdoc />
        public WorldEntityType Type => WorldEntityType.Player;
        
        /// <summary>
        /// Creates a new <see cref="RecoverySystem"/> instance.
        /// </summary>
        public RecoverySystem()
        {
            this._logger = DependencyContainer.Instance.Resolve<ILogger<RecoverySystem>>();
        }

        /// <inheritdoc />
        public void Execute(IEntity entity, SystemEventArgs args)
        {
            if (!(entity is IPlayerEntity player))
            {
                this._logger.LogError($"Cannot execute Recovery System. {entity.Object.Name} is not a player.");
                return;
            }

            if (!args.CheckArguments())
            {
                this._logger.LogError($"Cannot execute Recovery System action: {args.GetType()} due to invalid arguments.");
                return;
            }

            switch (args)
            {
                case IdleRecoveryEventArgs e:
                    this.IdleHeal(player, e);
                    break;
                default:
                    this._logger.LogWarning($"Unknown recovery system action type: {args.GetType()} for player {entity.Object.Name}");
                    break;
            }
        }

        /// <summary>
        /// Process the idle heal.
        /// </summary>
        /// <param name="player">Player entity.</param>
        /// <param name="e">Idle recovery event args.</param>
        private void IdleHeal(IPlayerEntity player, IdleRecoveryEventArgs e)
        {
            if (e.IsSitted)
            {
                player.Timers.NextHealTime += Time.TimeInSeconds() + NextIdleHealSit;
            }
            else
            {
                player.Timers.NextHealTime += Time.TimeInSeconds() + NextIdleHealStand;
            }

            if (player.Health.IsDead)
                return;

            int maxHp = HealthFormulas.GetMaxOriginHp(player.Object.Level, player.Statistics.Stamina, player.PlayerData.JobData.MaxHpFactor);
            int maxMp = HealthFormulas.GetMaxOriginMp(player.Object.Level, player.Statistics.Intelligence, player.PlayerData.JobData.MaxMpFactor, true);
            int maxFp = HealthFormulas.GetMaxOriginFp(player.Object.Level, player.Statistics.Stamina, player.Statistics.Dexterity, player.Statistics.Strength, player.PlayerData.JobData.MaxFpFactor, true);
            int recoveryHp = HealthFormulas.GetHpRecovery(maxHp, player.Object.Level, player.Statistics.Stamina, player.PlayerData.JobData.HpRecoveryFactor);
            int recoveryMp = HealthFormulas.GetMpRecovery(maxMp, player.Object.Level, player.Statistics.Intelligence, player.PlayerData.JobData.MpRecoveryFactor);
            int recoveryFp = HealthFormulas.GetFpRecovery(maxFp, player.Object.Level, player.Statistics.Stamina, player.PlayerData.JobData.FpRecoveryFactor);

            // TODO: set player hp, mp, fp and send packet
        }
    }
}
