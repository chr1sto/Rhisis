using Rhisis.World.Game.Entities;

namespace Rhisis.World.Game.Behaviors
{
    /// <summary>
    /// Default behavior for all monsters.
    /// </summary>
    [Behavior(BehaviorType.Monster, IsDefault: true)]
    public class DefaultMonsterBehavior : IBehavior<IMonsterEntity>
    {
        public virtual void Update(IMonsterEntity entity)
        {
            // TODO: update IA
        }
    }

    /// <summary>
    /// Behavior for Clockwork. (MI_CLOCKWORK1)
    /// </summary>
    [Behavior(BehaviorType.Monster, 164)]
    public class ClocksworkBehavior : IBehavior<IMonsterEntity>
    {
        public void Update(IMonsterEntity entity)
        {
            // TODO: implement Clockwork's IA
        }
    }
}
