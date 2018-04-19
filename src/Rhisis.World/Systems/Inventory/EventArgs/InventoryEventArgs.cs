using Rhisis.World.Game.Core;

namespace Rhisis.World.Systems.Inventory.EventArgs
{
    public class InventoryEventArgs : SystemEventArgs
    {
        /// <summary>
        /// Gets the <see cref="InventoryEventArgs"/> action type to execute.
        /// </summary>
        public InventoryActionType ActionType { get; }

        /// <summary>
        /// Creates a new <see cref="InventoryEventArgs"/> instance.
        /// </summary>
        /// <param name="type">Action type to execute</param>
        /// <param name="args">Optional arguments</param>
        public InventoryEventArgs(InventoryActionType type, params object[] args)
            : base(args)
        {
            this.ActionType = type;
        }

        /// <inheritdoc />
        public override bool CheckArguments() => true;
    }
}
