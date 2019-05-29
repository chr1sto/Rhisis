using Rhisis.World.Game.Core;

namespace Rhisis.World.Game.Behaviors
{
    /// <summary>
    /// Describes the behavior of an AI (Behavior)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBehavior<in T> where T : IEntity
    {
        /// <summary>
        /// Updates the AI/Behavior.
        /// </summary>
        /// <param name="entity">Entity to update</param>
        void Update(T entity);

        /// <summary>
        /// Process an action when the entity arrives to its destination.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        void OnArrived(T entity);
    }
}
