using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems
{
    [System]
    public sealed class BehaviorSystem : ISystem
    {
        /// <inheritdoc />
        public WorldEntityType Type => WorldEntityType.Player | WorldEntityType.Monster | WorldEntityType.Npc;

        /// <inheritdoc />
        public void Execute(IEntity entity, SystemEventArgs args)
        {
            switch (entity)
            {
                case IMonsterEntity monster:
                    monster.Behavior?.Update(monster);
                    break;
                case INpcEntity npc:
                    npc.Behavior?.Update(npc);
                    break;
                case IPlayerEntity player:
                    player.Behavior?.Update(player);
                    break;
            }
        }
    }
}
