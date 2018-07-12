using Rhisis.World.Game.Entities;

namespace Rhisis.World.Game.Behaviors
{
    /// <summary>
    /// Default behavior of a NPC.
    /// </summary>
    [Behavior(BehaviorType.Npc, IsDefault: true)]
    public class DefaultNpcBehavior : IBehavior<INpcEntity>
    {
        /// <inheritdoc />
        public void Update(INpcEntity entity)
        {
            // TODO: implement NPC IA.
        }
    }
}
