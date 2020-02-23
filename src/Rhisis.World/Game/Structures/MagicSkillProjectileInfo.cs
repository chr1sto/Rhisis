using Rhisis.World.Game.Entities;
using System;

namespace Rhisis.World.Game.Structures
{
    public class MagicSkillProjectileInfo : ProjectileInfo
    {
        /// <summary>
        /// Gets the projectile skill.
        /// </summary>
        public SkillInfo Skill { get; }

        /// <summary>
        /// Creates a new <see cref="MagicSkillProjectileInfo"/> instance.
        /// </summary>
        /// <param name="id">Projectile id.</param>
        /// <param name="owner">Projectile owner entity.</param>
        /// <param name="target">Projectile target entity.</param>
        /// <param name="skill">Projectile skill.</param>
        /// <param name="onArrived">Projection action to execute once arrived.</param>
        public MagicSkillProjectileInfo(int id, ILivingEntity owner, ILivingEntity target, SkillInfo skill, Action onArrived) 
            : base(id, owner, target, onArrived)
        {
            Skill = skill;
        }
    }
}