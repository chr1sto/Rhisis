using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core.Interfaces;

namespace Rhisis.World.Game.Entities
{
    public interface IPlayerEntity : IEntity
    {
        HumanComponent HumanComponent { get; set; }

        PlayerComponent PlayerComponent { get; set; }

        MovableComponent MovableComponent { get; set; }
    }
}
