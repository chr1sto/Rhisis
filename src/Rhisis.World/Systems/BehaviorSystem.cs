using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems
{
    [System]
    public sealed class BehaviorSystem : SystemBase
    {
        /// <inheritdoc />
        protected override WorldEntityType Type => WorldEntityType.Player | WorldEntityType.Monster | WorldEntityType.Npc;

        /// <summary>
        /// Creates a new <see cref="BehaviorSystem"/> instance.
        /// </summary>
        /// <param name="context"></param>
        public BehaviorSystem(IContext context) 
            : base(context)
        {
        }

        /// <inheritdoc />
        public override void Execute(IEntity entity)
        {
            switch (entity)
            {
                case IMonsterEntity monster:
                    monster.Behavior.Update(monster);
                    break;
                case INpcEntity npc:
                    npc.Behavior.Update(npc);
                    break;
                case IPlayerEntity player:
                    player.Behavior.Update(player);
                    break;
            }
        }
    }
}
