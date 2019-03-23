using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;

namespace Rhisis.World.Game.Entities
{
    public interface IMovableEntity : IEntity
    {
        MovableComponent MovableComponent { get; set; }

        FollowComponent Follow { get; set; }
    }
}
