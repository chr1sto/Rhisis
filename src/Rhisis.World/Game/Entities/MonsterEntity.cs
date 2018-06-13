using Rhisis.World.Game.Behaviors;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;

namespace Rhisis.World.Game.Entities
{
    public class MonsterEntity : Entity, IMonsterEntity
    {
        public override WorldEntityType Type => WorldEntityType.Monster;

        public MovableComponent MovableComponent { get; set; }

        public IBehavior<IMonsterEntity> Behavior { get; set; }

        public MonsterEntity(IContext context)
            : base(context)
        {
            this.MovableComponent = new MovableComponent();
        }
    }
}
