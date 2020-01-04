using Rhisis.World.Game.Entities;

namespace Rhisis.World.Game
{
    /// <summary>
    /// Provides an interface used for initializing and saving different components of a player.
    /// </summary>
    public interface IGameSystemLifeCycle
    {
        /// <summary>
        /// Gets the execution order.
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Initialize the player.
        /// </summary>
        /// <param name="player">Current player.</param>
        void Initialize(IPlayerEntity player);

        /// <summary>
        /// Save the player.
        /// </summary>
        /// <param name="player">Current player.</param>
        void Save(IPlayerEntity player);
    }
}
