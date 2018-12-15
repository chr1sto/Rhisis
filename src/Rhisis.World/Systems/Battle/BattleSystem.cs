using NLog;
using Rhisis.Core.IO;
using Rhisis.World.Game.Common;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;

namespace Rhisis.World.Systems.Battle
{
    [System(SystemType.Notifiable)]
    public class BattleSystem : ISystem
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public WorldEntityType Type => WorldEntityType.Player | WorldEntityType.Monster;

        /// <inheritdoc />
        public void Execute(IEntity entity, SystemEventArgs args)
        {
            if (!args.CheckArguments())
            {
                Logger.Error("Cannot execute battle action: {0} due to invalid arguments.", args.GetType());
                return;
            }

            if (!(entity is ILivingEntity livingEntity))
            {
                Logger.Error($"The non living entity {entity.Object.Name} tried to execute a battle action.");
                return;
            }

            switch (args)
            {
                case MeleeAttackEventArgs meleeAttackEventArgs:
                    this.ProcessMeleeAttack(livingEntity, meleeAttackEventArgs);
                    break;
            }
        }

        /// <summary>
        /// Process the melee attack algorithm.
        /// </summary>
        /// <param name="attacker">Attacker</param>
        /// <param name="e">Melee attack event arguments</param>
        private void ProcessMeleeAttack(ILivingEntity attacker, MeleeAttackEventArgs e)
        {
            ILivingEntity defender = e.Target;

            if (defender.Health.IsDead)
            {
                Logger.Error($"{attacker.Object.Name} cannot attack {defender.Object.Name} because target is already dead.");
                return;
            }

            AttackResult meleeAttackResult = new MeleeAttackArbiter(attacker, defender).OnDamage();

            Logger.Debug($"{attacker.Object.Name} inflicted {meleeAttackResult.Damages} to {defender.Object.Name}");

            if (!(attacker is IPlayerEntity player))
                return;

            if (meleeAttackResult.Flags.HasFlag(AttackFlags.AF_FLYING))
                BattleHelper.KnockbackEntity(defender);

            WorldPacketFactory.SendAddDamage(player, defender, attacker, meleeAttackResult.Flags, meleeAttackResult.Damages);

            defender.Health.Hp -= meleeAttackResult.Damages;

            if (defender.Health.IsDead)
            {
                Logger.Debug($"{attacker.Object.Name} killed {defender.Object.Name}.");
                defender.Health.Hp = 0;
                defender.Follow.Target = null;
                defender.Battle.Target = null;
                defender.Battle.Targets.Clear();
                attacker.Follow.Target = null;
                attacker.Battle.Target = null;
                attacker.Battle.Targets.Clear();
                WorldPacketFactory.SendDie(player, defender, attacker, e.AttackType);

                if (defender is IMonsterEntity deadMonster)
                {
                    deadMonster.Timers.DespawnTime = Time.TimeInSeconds() + 5;
                    // TODO: give exp and drop items.
                }
            }
        }
    }
}
