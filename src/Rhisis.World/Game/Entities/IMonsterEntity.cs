using Rhisis.World.Game.Behaviors;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Maps.Regions;

namespace Rhisis.World.Game.Entities
{
    public interface IMonsterEntity : IEntity, IMovableEntity
    {
        /// <summary>
        /// Gets or sets the parent region of the monster.
        /// </summary>
        IMapRegion Region { get; set; }

        /// <summary>
        /// Gets the monster's behavior.
        /// </summary>
        IBehavior<IMonsterEntity> Behavior { get; set; }

        /// <summary>
        /// Gets the monster's timer component.
        /// </summary>
        TimerComponent TimerComponent { get; set; }
    }
}
