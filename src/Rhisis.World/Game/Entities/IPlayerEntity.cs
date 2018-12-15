using Ether.Network.Common;
using Rhisis.World.Game.Behaviors;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;

namespace Rhisis.World.Game.Entities
{
    public interface IPlayerEntity : IEntity, IMovableEntity, ILivingEntity
    {
        VisualAppearenceComponent VisualAppearance { get; set; }

        PlayerDataComponent PlayerData { get; set; }

        ItemContainerComponent Inventory { get; set; }

        TradeComponent Trade { get; set; }
        
        NetUser Connection { get; set; }

        IBehavior<IPlayerEntity> Behavior { get; set; }
    }
}
