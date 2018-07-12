using Rhisis.World.Game.Behaviors;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Regions;

namespace Rhisis.World.Game.Entities
{
    public interface IMonsterEntity : IEntity, IMovableEntity
    {
        /// <summary>
        /// Gets or sets the parent region of the monster.
        /// </summary>
        IRegion Region { get; set; }

        /// <summary>
        /// Gets the monster's behavior.
        /// </summary>
        IBehavior<IMonsterEntity> Behavior { get; set; }

        /// <summary>
        /// Gets the monster's behavior.
        /// </summary>
        TimerComponent TimerComponent { get; set; }
    }
}
