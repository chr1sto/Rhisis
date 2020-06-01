using Microsoft.Extensions.Logging;
using Rhisis.Core.Common.Formulas;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.IO;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;

namespace Rhisis.World.Systems.Recovery
{
    /// <summary>
    /// Game system that manages all recoveries. HP, MP, FP, etc...
    /// </summary>
    [Injectable]
    public sealed class RecoverySystem : IRecoverySystem
    {
        /// <summary>
        /// Gets the number of seconds for next idle heal when the player is sitted.
        /// </summary>
        public const int NextIdleHealSit = 2;

        /// <summary>
        /// Gets the number of seconds for the idle heal when the player stands up.
        /// </summary>
        public const int NextIdleHealStand = 3;

        private readonly ILogger<RecoverySystem> _logger;
        private readonly IMoverPacketFactory _moverPacketFactory;

        /// <summary>
        /// Creates a new <see cref="RecoverySystem"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="moverPacketFactory">Mover packet factory.</param>
        public RecoverySystem(ILogger<RecoverySystem> logger, IMoverPacketFactory moverPacketFactory)
        {
            _logger = logger;
            _moverPacketFactory = moverPacketFactory;
        }

        /// <inheritdoc />
        public void IdleRecevory(IPlayerEntity player, bool isSitted = false)
        {
            int nextHealDelay = isSitted ? NextIdleHealSit : NextIdleHealStand;

            player.Timers.NextHealTime = Time.TimeInSeconds() + nextHealDelay;

            if (player.IsDead)
            {
                return;
            }

            int strength = player.Attributes[DefineAttributes.STR];
            int stamina = player.Attributes[DefineAttributes.STA];
            int dexterity = player.Attributes[DefineAttributes.DEX];
            int intelligence = player.Attributes[DefineAttributes.INT];

            int maxHp = HealthFormulas.GetMaxOriginHp(player.Object.Level, stamina, player.PlayerData.JobData.MaxHpFactor);
            int maxMp = HealthFormulas.GetMaxOriginMp(player.Object.Level, intelligence, player.PlayerData.JobData.MaxMpFactor, true);
            int maxFp = HealthFormulas.GetMaxOriginFp(player.Object.Level, stamina, dexterity, strength, player.PlayerData.JobData.MaxFpFactor, true);
            int recoveryHp = HealthFormulas.GetHpRecovery(maxHp, player.Object.Level, stamina, player.PlayerData.JobData.HpRecoveryFactor);
            int recoveryMp = HealthFormulas.GetMpRecovery(maxMp, player.Object.Level, intelligence, player.PlayerData.JobData.MpRecoveryFactor);
            int recoveryFp = HealthFormulas.GetFpRecovery(maxFp, player.Object.Level, stamina, player.PlayerData.JobData.FpRecoveryFactor);

            player.Attributes[DefineAttributes.HP] += recoveryHp;
            player.Attributes[DefineAttributes.MP] += recoveryMp;
            player.Attributes[DefineAttributes.FP] += recoveryFp;

            if (player.Attributes[DefineAttributes.HP] > maxHp)
                player.Attributes[DefineAttributes.HP] = maxHp;
            if (player.Attributes[DefineAttributes.MP] > maxMp)
                player.Attributes[DefineAttributes.MP] = maxMp;
            if (player.Attributes[DefineAttributes.FP] > maxFp)
                player.Attributes[DefineAttributes.FP] = maxFp;

            _moverPacketFactory.SendUpdateAttributes(player, DefineAttributes.HP, player.Attributes[DefineAttributes.HP]);
            _moverPacketFactory.SendUpdateAttributes(player, DefineAttributes.MP, player.Attributes[DefineAttributes.MP]);
            _moverPacketFactory.SendUpdateAttributes(player, DefineAttributes.FP, player.Attributes[DefineAttributes.FP]);
        }
    }
}
