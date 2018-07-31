using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;
using System;
using System.Linq.Expressions;

namespace Rhisis.World.Systems
{
    [System]
    public sealed class BehaviorSystem : SystemBase
    {
        /// <inheritdoc />
        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.Player;

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
            // TODO: instead of updating every monster of each player
            // we need to setup map chunks that contains the monster
            // references so we just need them instead of looping
            // through the player's list
            foreach (var otherEntity in entity.Object.Entities)
            {
                switch (otherEntity)
                {
                    case IMonsterEntity monster:
                        monster.Behavior.Update(monster);
                        break;
                    case INpcEntity npc:
                        npc.Behavior.Update(npc);
                        break;
                    case IPlayerEntity player:

                        break;
                }
            }

            
        }
    }
}
