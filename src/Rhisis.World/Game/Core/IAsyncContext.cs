namespace Rhisis.World.Game.Core
{
    /// <summary>
    /// Defines an asynchronous context.
    /// </summary>
    public interface IAsyncContext : IContext
    {
        /// <summary>
        /// Starts a context in a parallel task.
        /// </summary>
        /// <param name="delay"></param>
        void StartUpdateTask(int delay);

        /// <summary>
        /// Stops the context and the task.
        /// </summary>
        void StopUpdateTask();
    }
}