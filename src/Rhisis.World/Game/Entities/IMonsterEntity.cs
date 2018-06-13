using Rhisis.World.Game.Behaviors;
using Rhisis.World.Game.Core.Interfaces;

namespace Rhisis.World.Game.Entities
{
    public interface IMonsterEntity : IEntity, IMovableEntity
    {
        IBehavior<IMonsterEntity> Behavior { get; set; }
    }
}
