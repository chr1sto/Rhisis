using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;

namespace Rhisis.World.Systems.Battle
{
    public class MeleeAttackEventArgs : SystemEventArgs
    {
        public int AttackType { get; }

        public IEntity Target { get; }

        public float AttackSpeed { get; }

        public MeleeAttackEventArgs(int attackType, IEntity target, float attackSpeed)
        {
            this.AttackType = attackType;
            this.Target = target;
            this.AttackSpeed = attackSpeed;
        }

        public override bool CheckArguments()
        {
            return this.Target != null; // TODO: check if target is alive
        }
    }
}
