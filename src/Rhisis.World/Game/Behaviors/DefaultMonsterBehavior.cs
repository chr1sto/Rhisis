using Rhisis.World.Game.Entities;

namespace Rhisis.World.Game.Behaviors
{
    /// <summary>
    /// Default behavior for all monsters.
    /// </summary>
    [Behavior]
    public class DefaultMonsterBehavior : IBehavior<IMonsterEntity>
    {
        public void Update(IMonsterEntity entity)
        {
            // TODO: update IA
        }
    }

    /// <summary>
    /// Behavior for Clockwork. (MI_CLOCKWORK1)
    /// </summary>
    [Behavior(164)]
    public class ClocksworkBehavior : IBehavior<IMonsterEntity>
    {
        public void Update(IMonsterEntity entity)
        {
            // TODO: implement Clockwork's IA
        }
    }

    /// <summary>
    /// Behavior for small, normal and captain aibatts.
    /// MI_AIBATT1, MI_AIBATT2, MI_AIBATT3
    /// </summary>
    [Behavior(20)] // Small aibatt
    [Behavior(21)] // Aibatt
    [Behavior(22)] // Captain aibatt
    public class AibattBehavior : IBehavior<IMonsterEntity>
    {
        public void Update(IMonsterEntity entity)
        {
            // TODO: implement Aibatts behavior. (small, normal, and captain aibatts)
            // Defined with the behavior attribute
        }
    }
}
