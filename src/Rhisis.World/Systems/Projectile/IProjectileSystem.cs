using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Systems.Projectile
{
    /// <summary>
    /// Provides a mechanism to manage projectiles.
    /// </summary>
    public interface IProjectileSystem
    {
        /// <summary>
        /// Creates a new projectile.
        /// </summary>
        /// <param name="projectile">Projectile information.</param>
        /// <returns>Projectile id.</returns>
        int CreateProjectile(ProjectileInfo projectile);

        /// <summary>
        /// Removes a projectile from the given living entity projectile's collection.
        /// </summary>
        /// <param name="livingEntity">Current living entity.</param>
        /// <param name="projectileId">Projectile id to remove.</param>
        void RemoveProjectile(ILivingEntity livingEntity, int projectileId);

        /// <summary>
        /// Gets a projectile from a given living entity by its id and converts it into a <typeparamref name="TProjectile"/>.
        /// </summary>
        /// <typeparam name="TProjectile">Projectile type.</typeparam>
        /// <param name="livingEntity">Current living entity.</param>
        /// <param name="projectileId">Projectile id.</param>
        /// <returns>Projectile.</returns>
        TProjectile GetProjectile<TProjectile>(ILivingEntity livingEntity, int projectileId) where TProjectile : ProjectileInfo;
    }
}
