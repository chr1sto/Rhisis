using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;

namespace Rhisis.World.Game.Entities
{
    public class NpcEntity : Entity, INpcEntity
    {
        public override WorldEntityType Type => WorldEntityType.Npc;

        public NpcEntity(IContext context)
            : base(context)
        {
        }
    }
}
