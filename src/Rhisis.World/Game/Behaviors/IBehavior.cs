using Rhisis.World.Game.Entities;

namespace Rhisis.World.Game.Behaviors
{
    /// <summary>
    /// Provides a mechanism to manage an entity behavior.
    /// </summary>
    public interface IBehavior
    {
        /// <summary>
        /// Updates the AI/Behavior.
        /// </summary>
        void Update();

        /// <summary>
        /// Process an action when the entity arrives to its destination.
        /// </summary>
        void OnArrived();

        /// <summary>
        /// Process an action when an entity is killed.
        /// </summary>
        /// <param name="killedEntity">Killed entity.</param>
        void OnTargetKilled(ILivingEntity killedEntity);

        /// <summary>
        /// Process an action when the current entity is killed.
        /// </summary>
        /// <param name="killerEntity">Killer.</param>
        void OnKilled(ILivingEntity killerEntity);
    }
}
