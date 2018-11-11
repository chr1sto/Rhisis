using NLog;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Entities;
using System;

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

        private void ProcessMeleeAttack(ILivingEntity attacker, MeleeAttackEventArgs e)
        {
            if (!e.Target.Health.IsDead)
            {
                Logger.Error($"{attacker.Object.Name} cannot attack {e.Target.Object.Name} because target is already dead.");
                return;
            }

            int attackDamages = BattleHelper.GetMeeleAttackDamages(attacker, e.Target);

            Logger.Debug($"{attacker.Object.Name} inflicted {attackDamages} to {e.Target.Object.Name}");
        }
    }
}
