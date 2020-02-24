using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Systems.Projectile
{
    [Injectable]
    public class ProjectileSystem : IProjectileSystem
    {
        /// <inheritdoc />
        public int CreateProjectile(ProjectileInfo projectile)
        {
            ILivingEntity livingEntity = projectile.Owner;
            int projectileId = livingEntity.Battle.LastProjectileId++;

            livingEntity.Battle.Projectiles.Add(projectileId, projectile);

            return projectileId;
        }

        /// <inheritdoc />
        public void RemoveProjectile(ILivingEntity livingEntity, int projectileId)
        {
            livingEntity.Battle.Projectiles.Remove(projectileId);
        }

        /// <inheritdoc />
        public TProjectile GetProjectile<TProjectile>(ILivingEntity livingEntity, int projectileId) 
            where TProjectile : ProjectileInfo 
            => livingEntity.Battle.Projectiles.TryGetValue(projectileId, out ProjectileInfo projectile) ? (TProjectile)projectile : default;
    }
}
