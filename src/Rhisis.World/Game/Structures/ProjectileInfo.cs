using Rhisis.World.Game.Entities;
using System;

namespace Rhisis.World.Game.Structures
{
    public class ProjectileInfo
    {
        /// <summary>
        /// Gets the projectile id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the projectile owner.
        /// </summary>
        public ILivingEntity Owner { get; }

        /// <summary>
        /// Gets the projectile target.
        /// </summary>
        public ILivingEntity Target { get; }

        /// <summary>
        /// Gets the action to execute once the projectile has arrived to its target destination.
        /// </summary>
        public Action OnArrived { get; }

        /// <summary>
        /// Creates a new <see cref="ProjectileInfo"/> instance.
        /// </summary>
        /// <param name="id">Projectile id.</param>
        /// <param name="owner">Projectile owner entity.</param>
        /// <param name="target">Projectile target entity.</param>
        /// <param name="onArrived">Projection action to execute once arrived.</param>
        public ProjectileInfo(int id, ILivingEntity owner, ILivingEntity target, Action onArrived)
        {
            Id = id;
            Owner = owner;
            Target = target;
            OnArrived = onArrived;
        }
    }
}