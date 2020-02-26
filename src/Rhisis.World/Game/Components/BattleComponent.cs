using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Components
{
    public class BattleComponent
    {
        /// <summary>
        /// Gets or sets the fighting target.
        /// </summary>
        public ILivingEntity Target { get; set; }

        /// <summary>
        /// Gets or sets multiple fighting targets.
        /// </summary>
        public IList<ILivingEntity> Targets { get; set; }

        /// <summary>
        /// Gets a value that indicates if the object is currently fighting.
        /// </summary>
        public bool IsFighting => Target != null || Targets.Any();

        /// <summary>
        /// Gets the collection of active projectiles.
        /// </summary>
        public IDictionary<int, ProjectileInfo> Projectiles { get; }

        /// <summary>
        /// Gets the last projectile id.
        /// </summary>
        public int LastProjectileId { get; set; } = 1;

        /// <summary>
        /// Creates and initializes the <see cref="BattleComponent"/>.
        /// </summary>
        public BattleComponent()
        {
            Targets = new List<ILivingEntity>();
            Projectiles = new Dictionary<int, ProjectileInfo>();
        }

        /// <summary>
        /// Reset the battle component to its initial state.
        /// </summary>
        public void Reset()
        {
            Target = null;
            Targets.Clear();
        }
    }
}
