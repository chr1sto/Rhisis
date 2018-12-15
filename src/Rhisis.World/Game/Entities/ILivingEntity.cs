using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;

namespace Rhisis.World.Game.Entities
{
    public interface ILivingEntity : IEntity, IMovableEntity
    {
        /// <summary>
        /// Gets or sets the interaction component.
        /// </summary>
        InteractionComponent Interaction { get; set; }

        /// <summary>
        /// Gets or sets the battle component.
        /// </summary>
        BattleComponent Battle { get; set; }

        /// <summary>
        /// Gets or sets the Health component.
        /// </summary>
        HealthComponent Health { get; set; }

        /// <summary>
        /// Gets or sets the statistics component.
        /// </summary>
        StatisticsComponent Statistics { get; set; }
    }
}
