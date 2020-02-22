using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Behaviors;
using Rhisis.World.Game.Components;

namespace Rhisis.World.Game.Entities
{
    public interface ILivingEntity : IWorldEntity, IMovableEntity
    {
        /// <summary>
        /// Gets a value that indicates if the living entity is dead.
        /// </summary>
        bool IsDead { get; }

        /// <summary>
        /// Gets or sets the living entity mover data.
        /// </summary>
        MoverData Data { get; set; }

        /// <summary>
        /// Gets or sets the interaction component.
        /// </summary>
        InteractionComponent Interaction { get; set; }

        /// <summary>
        /// Gets or sets the battle component.
        /// </summary>
        BattleComponent Battle { get; set; }

        /// <summary>
        /// Gets or sets the attribute component.
        /// </summary>
        AttributeComponent Attributes { get; set; }

        /// <summary>
        /// Gets or sets the living entity behavior.
        /// </summary>
        IBehavior Behavior { get; set; }
    }
}
