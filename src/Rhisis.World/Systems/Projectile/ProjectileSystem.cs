using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Systems.Projectile
{
    [Injectable]
    public class ProjectileSystem : IProjectileSystem
    {
        /// <inheritdoc />
        public void CreateProjectile(ProjectileInfo projectile)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void ExecuteProjectile(int projectileId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public TProjectile GetProjectile<TProjectile>(int projectileId) where TProjectile : ProjectileInfo
        {
            throw new NotImplementedException();
        }
    }
}
