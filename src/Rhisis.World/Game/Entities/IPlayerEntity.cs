using Ether.Network;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core.Interfaces;

namespace Rhisis.World.Game.Entities
{
    public interface IPlayerEntity : IEntity, IMovableEntity
    {
        HumanComponent HumanComponent { get; set; }

        PlayerComponent PlayerComponent { get; set; }

        ItemContainerComponent InventoryComponent { get; set; }
        
        NetConnection Connection { get; set; }
    }
}
