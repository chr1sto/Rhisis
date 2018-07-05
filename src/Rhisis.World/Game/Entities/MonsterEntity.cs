using Rhisis.World.Game.Behaviors;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Regions;

namespace Rhisis.World.Game.Entities
{
    /// <summary>
    /// Describes the Monster entity.
    /// </summary>
    public class MonsterEntity : Entity, IMonsterEntity
    {
        /// <inheritdoc />
        public override WorldEntityType Type => WorldEntityType.Monster;

        /// <inheritdoc />
        public IRegion Region { get; set; }

        /// <inheritdoc />
        public MovableComponent MovableComponent { get; set; }

        /// <inheritdoc />
        public IBehavior<IMonsterEntity> Behavior { get; set; }

        /// <inheritdoc />
        public TimerComponent TimerComponent { get; set; }

        /// <summary>
        /// Creates a new <see cref="MonsterEntity"/> instance.
        /// </summary>
        /// <param name="context"></param>
        public MonsterEntity(IContext context)
            : base(context)
        {
            this.MovableComponent = new MovableComponent();
            this.TimerComponent = new TimerComponent();
        }
    }
}
