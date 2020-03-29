using Rhisis.Core.Common;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Maps.Regions;

namespace Rhisis.World.Game.Entities.Internal
{
    public class ItemEntity : WorldEntity, IItemEntity
    {
        public DropComponent Drop { get; set; }

        public override WorldEntityType Type => WorldEntityType.Drop;

        public IMapRespawnRegion Region { get; set; }

        public ItemEntity()
        {
            Object.Type = WorldObjectType.Item;
            Drop = new DropComponent();
        }
    }
}
