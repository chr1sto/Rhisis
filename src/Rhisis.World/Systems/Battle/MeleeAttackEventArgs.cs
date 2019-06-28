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

        public int UnknownParameter { get; }

        public MeleeAttackEventArgs(ObjectMessageType attackType, ILivingEntity target, float attackSpeed)
            : this(attackType, target, attackSpeed, 0)
        {
        }

        public MeleeAttackEventArgs(ObjectMessageType attackType, ILivingEntity target, float attackSpeed, int unknownParameter)
        {
            this.AttackType = attackType;
            this.Target = target;
            this.AttackSpeed = attackSpeed;
            this.UnknownParameter = unknownParameter;
        }

        public override bool GetCheckArguments()
        {
            return this.Target != null; // TODO: check if target is alive
        }
    }
}
