using Ether.Network.Common;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;

namespace Rhisis.World.Game.Entities
{
    public interface IPlayerEntity : IEntity, IMovableEntity
    {
        VisualAppearenceComponent VisualAppearance { get; set; }

        PlayerComponent PlayerData { get; set; }

        ItemContainerComponent Inventory { get; set; }

        StatisticsComponent Statistics { get; set; }

        TradeComponent Trade { get; set; }
        
        NetUser Connection { get; set; }
    }
}
