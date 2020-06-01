using Rhisis.World.Game.Common;
using Rhisis.World.Game.Entities;
using System;

namespace Rhisis.World.Game.Structures
{
    public class MagicSkillProjectileInfo : ProjectileInfo
    {
        /// <inheritdoc />
        public override AttackFlags Type => AttackFlags.AF_MAGICSKILL;

        /// <summary>
        /// Gets the projectile skill.
        /// </summary>
        public SkillInfo Skill { get; }

        /// <summary>
        /// Creates a new <see cref="MagicSkillProjectileInfo"/> instance.
        /// </summary>
        /// <param name="owner">Projectile owner entity.</param>
        /// <param name="target">Projectile target entity.</param>
        /// <param name="skill">Projectile skill.</param>
        /// <param name="onArrived">Action to execute when the magic attack projectile arrives to its target.</param>
        public MagicSkillProjectileInfo(ILivingEntity owner, ILivingEntity target, SkillInfo skill, Action onArrived) 
            : base(owner, target, onArrived)
        {
            Skill = skill;
        }
    }
}