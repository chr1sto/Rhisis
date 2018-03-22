using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.Statistics
{
    public class StatisticsEventArgs : SystemEventArgs
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
            : base(args)
        {
            this.ActionType = type;
            this.Arguments = args;
        }

        /// <inheritdoc />
        public override bool CheckArguments()
        {
            // TODO
            return true;
        }
    }
}