using Ether.Network.Common;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;

namespace Rhisis.World.Game.Entities
{
    public class PlayerEntity : Entity, IPlayerEntity
    {
        public override WorldEntityType Type => WorldEntityType.Player;
        
        public HumanComponent HumanComponent { get; set; }

        public PlayerComponent PlayerComponent { get; set; }

        public MovableComponent MovableComponent { get; set; }

        public ItemContainerComponent Inventory { get; set; }

        public StatisticsComponent StatisticsComponent { get; set; }

        public TradeComponent Trade { get; set; }

        public NetUser Connection { get; set; }

        public PlayerEntity(IContext context)
            : base(context)
        {
        }
    }
}
