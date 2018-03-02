using System;

namespace Rhisis.World.Systems.Events.Statistics
{
    public class StatisticsEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="StatisticsActionType"/> action type to execute.
        /// </summary>
        public StatisticsActionType ActionType { get; }

        /// <summary>
        /// Gets the <see cref="StatisticsActionType"/> optional arguments.
        /// </summary>
        public object[] Arguments { get; }

        /// <summary>
        /// Creates a new <see cref="StatisticsActionType"/> instance.
        /// </summary>
        /// <param name="type">Action type to execute</param>
        /// <param name="args">Optional arguments</param>
        public StatisticsEventArgs(StatisticsActionType type, params object[] args)
        {
            this.ActionType = type;
            this.Arguments = args;
        }
    }
}