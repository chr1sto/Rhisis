namespace Rhisis.World
{
    /// <summary>
    /// Provides a mechanism to manage the world server running tasks.
    /// </summary>
    public interface IWorldServerTaskManager
    {
        /// <summary>
        /// Starts the world server task manager process.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the world server task manager process.
        /// </summary>
        void Stop();
    }
}
