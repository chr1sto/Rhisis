using Rhisis.World.Game.Common;
using Rhisis.World.Game.Entities;
using System;

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
        /// Gets the action to execute when the projectile arrives to its target.
        /// </summary>
        public Action OnArrived { get; }

        /// <summary>
        /// Gets or sets the projectile attack type.
        /// </summary>
        public abstract AttackFlags Type { get; }

        /// <summary>
        /// Creates a new <see cref="ProjectileInfo"/> instance.
        /// </summary>
        /// <param name="owner">Projectile owner entity.</param>
        /// <param name="target">Projectile target entity.</param>
        /// <param name="onArrived">Action to execute when the projectile arrives to its target.</param>
        protected ProjectileInfo(ILivingEntity owner, ILivingEntity target, Action onArrived)
        {
            Owner = owner;
            Target = target;
            OnArrived = onArrived;
        }
    }
}