using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;

namespace Rhisis.World.Game.Entities
{
    public class MonsterEntity : Entity, IMonsterEntity
    {
        public override WorldEntityType Type => WorldEntityType.Monster;

        public MonsterEntity(IContext context)
            : base(context)
        {
        }
    }
}
