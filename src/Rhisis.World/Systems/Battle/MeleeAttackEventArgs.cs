using Rhisis.Core.Data;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems.Battle
{
    public class MeleeAttackEventArgs : SystemEventArgs
    {
        public ObjectMessageType AttackType { get; }

        public ILivingEntity Target { get; }

        public float AttackSpeed { get; }

        public MeleeAttackEventArgs(ObjectMessageType attackType, ILivingEntity target, float attackSpeed)
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
