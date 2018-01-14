using System;

namespace Rhisis.World.Systems.Events.Inventory
{
    public class InventoryEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="InventoryEventArgs"/> action type to execute.
        /// </summary>
        public InventoryActionType ActionType { get; }
        
        /// <summary>
        /// Gets the <see cref="InventoryEventArgs"/> optional arguments.
        /// </summary>
        public object[] Arguments { get; }

        /// <summary>
        /// Creates a new <see cref="InventoryEventArgs"/> instance.
        /// </summary>
        /// <param name="type">Action type to execute</param>
        /// <param name="args">Optional arguments</param>
        public InventoryEventArgs(InventoryActionType type, params object[] args)
        {
            this.ActionType = type;
            this.Arguments = args;
        }
    }
}
