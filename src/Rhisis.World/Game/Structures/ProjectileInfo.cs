using Rhisis.World.Game.Common;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Game.Structures
{
    public abstract class ProjectileInfo
    {
        /// <summary>
        /// Gets the projectile owner.
        /// </summary>
        public ILivingEntity Owner { get; }

        /// <summary>
        /// Gets the projectile target.
        /// </summary>
        public ILivingEntity Target { get; }

        /// <summary>
        /// Gets or sets the projectile attack type.
        /// </summary>
        public abstract AttackFlags Type { get; }

        /// <summary>
        /// Creates a new <see cref="ProjectileInfo"/> instance.
        /// </summary>
        /// <param name="owner">Projectile owner entity.</param>
        /// <param name="target">Projectile target entity.</param>
        protected ProjectileInfo(ILivingEntity owner, ILivingEntity target)
        {
            Owner = owner;
            Target = target;
        }
    }
}