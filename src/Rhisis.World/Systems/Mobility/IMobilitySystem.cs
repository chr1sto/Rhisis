using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems.Mobility
{
    /// <summary>
    /// Provides a mechanism to manage every feature related with moves.
    /// </summary>
    public interface IMobilitySystem
    {
        /// <summary>
        /// Calculates the movable entities positions in real-time.
        /// </summary>
        /// <param name="entity">Living and moving entity.</param>
        void CalculatePosition(ILivingEntity entity);
    }
}
