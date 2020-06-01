using Rhisis.Core.Common.Game.Structures;
using Rhisis.World.Game;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems.Taskbar
{
    public interface ITaskbarSystem : IGameSystemLifeCycle
    {
        /// <summary>
        /// Adds a new applet to the player's applet taskbar.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="shortcutToAdd">Shortcut to add.</param>
        void AddApplet(IPlayerEntity player, Shortcut shortcutToAdd);

        /// <summary>
        /// Adds an item to the player's item taskbar at a given slot level.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="shortcutToAdd">Shortcut to add.</param>
        /// <param name="slotLevelIndex">Item taskbar slot level index.</param>
        void AddItem(IPlayerEntity player, Shortcut shortcutToAdd, int slotLevelIndex);

        /// <summary>
        /// Removes an applet from the player's applet taskbar.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="slotIndex">Applet slot index.</param>
        void RemoveApplet(IPlayerEntity player, int slotIndex);

        /// <summary>
        /// Removes an item from the player's item taskbar at a given slot level index.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="slotLevelIndex">Item taskbar slot level index.</param>
        /// <param name="slotIndex">Item taskbar slot index.</param>
        void RemoveItem(IPlayerEntity player, int slotLevelIndex, int slotIndex);
    }
}