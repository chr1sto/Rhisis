using Rhisis.World.Game.Entities;
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

        public IList<ILivingEntity> Targets { get; set; }

        /// <summary>
        /// Gets a value that indicates if the object is currently fighting.
        /// </summary>
        public bool IsFighting => this.Target != null || this.Targets.Any();
    }
}
