using Rhisis.World.Game.Structures;

namespace Rhisis.World.Systems.Projectile
{
    public interface IProjectileSystem
    {
        void CreateProjectile(ProjectileInfo projectile);

        void ExecuteProjectile(int projectileId);

        TProjectile GetProjectile<TProjectile>(int projectileId) where TProjectile : ProjectileInfo;
    }
}
