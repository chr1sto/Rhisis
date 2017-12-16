namespace Rhisis.World.Game.Core.Interfaces
{
    /// <summary>
    /// Describes the behavior of a basic system.
    /// </summary>
    public interface ISystem
    {
        /// <summary>
        /// Executes the system logic for the entity passed as parameter.
        /// </summary>
        /// <param name="entity">Entity</param>
        void Execute(IEntity entity);

        /// <summary>
        /// Check if the entity matches the system filter predicate.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns></returns>
        bool Match(IEntity entity);
    }
}
