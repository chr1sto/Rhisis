using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.Core.Extensions
{
    /// <summary>
    /// Provides helpers to the <see cref="Task"/> object.
    /// </summary>
    public static class TaskHelper
    {
        /// <summary>
        /// Creates a new long running task repeated every X times on the default task scheduler.
        /// </summary>
        /// <param name="taskToExecute">Task</param>
        /// <param name="delay">Repeat time delay.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task</returns>
        public static Task CreateLongRunningTask(Func<Task> taskToExecute, TimeSpan delay, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await taskToExecute();
                    await Task.Delay(delay, cancellationToken);
                }
            },
            cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
    }
}
