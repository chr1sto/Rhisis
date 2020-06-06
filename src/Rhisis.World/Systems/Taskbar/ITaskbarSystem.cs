using Rhisis.World.Game;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Systems.Taskbar
{
    public interface ITaskbarSystem : IGameSystemLifeCycle
    {
        /// <summary>
        /// Adds a new applet to the player's applet taskbar.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="shortcutToAdd">Shortcut to add.</param>
        void AddShortcutToAppletTaskbar(IPlayerEntity player, Shortcut shortcutToAdd);

        /// <summary>
        /// Adds an item to the player's item taskbar at a given slot level.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="shortcutToAdd">Shortcut to add.</param>
        /// <param name="slotLevelIndex">Item taskbar slot level index.</param>
        void AddShortcutToItemTaskbar(IPlayerEntity player, Shortcut shortcutToAdd, int slotLevelIndex);

        /// <summary>
        /// Removes an applet from the player's applet taskbar.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="slotIndex">Applet slot index.</param>
        void RemoveShortcutFromAppletTaskbar(IPlayerEntity player, int slotIndex);

        /// <summary>
        /// Removes an item from the player's item taskbar at a given slot level index.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="slotLevelIndex">Item taskbar slot level index.</param>
        /// <param name="slotIndex">Item taskbar slot index.</param>
        void RemoveShortcutFromItemTaskbar(IPlayerEntity player, int slotLevelIndex, int slotIndex);
    }
}