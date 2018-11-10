using NLog;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;
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

            switch (args)
            {
                case MeleeAttackEventArgs meleeAttackEventArgs:
                    this.ProcessMeleeAttack(entity, meleeAttackEventArgs);
                    break;
            }
        }

        private void ProcessMeleeAttack(IEntity attacker, MeleeAttackEventArgs e)
        {
            Logger.Debug($"{attacker.Object.Name} is attacking {e.Target.Object.Name}");
        }
    }
}
