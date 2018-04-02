using System;
using Rhisis.World.Game.Core;
using Rhisis.World.Systems.Statistics;

namespace Rhisis.World.Systems.Trade
{
    public class TradeEventArgs : SystemEventArgs
    {
        /// <summary>
        /// Gets the <see cref="TradeActionType"/> action type to execute.
        /// </summary>
        public TradeActionType ActionType { get; }

        /// <summary>
        /// Gets the <see cref="TradeActionType"/> optional arguments.
        /// </summary>
        public object[] Arguments { get; }

        /// <summary>
        /// Creates a new <see cref="TradeActionType"/> instance.
        /// </summary>
        /// <param name="type">Action type to execute</param>
        /// <param name="args">Optional arguments</param>
        public TradeEventArgs(TradeActionType type, params object[] args)
            : base(args)
        {
            this.ActionType = type;
            this.Arguments = args;
        }

        /// <inheritdoc />
        public override bool CheckArguments() => true;
    }
}