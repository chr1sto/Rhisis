using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Taskbar
{
    [Injectable]
    public sealed class TaskbarSystem : ITaskbarSystem
    {
        public const int MaxTaskbarApplets = 18;
        public const int MaxTaskbarItems = 9;
        public const int MaxTaskbarItemLevels = 8;
        public const int MaxTaskbarQueue = 5;

        private readonly ILogger<TaskbarSystem> _logger;
        private readonly IRhisisDatabase _database;

        /// <inheritdoc />
        public int Order => 1;

        /// <summary>
        /// Creates a new <see cref="TaskbarSystem"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="database">Rhisis database access layer.</param>
        public TaskbarSystem(ILogger<TaskbarSystem> logger, IRhisisDatabase database)
        {
            _logger = logger;
            _database = database;
        }

        /// <inheritdoc />
        public void Initialize(IPlayerEntity player)
        {
            IEnumerable<DbShortcut> shortcuts = _database.TaskbarShortcuts.Where(x => x.CharacterId == player.PlayerData.Id).AsNoTracking().AsEnumerable();

            player.Taskbar.Applets = new TaskbarAppletContainerComponent(MaxTaskbarApplets);
            player.Taskbar.Items = new TaskbarItemContainerComponent(MaxTaskbarItems, MaxTaskbarItemLevels);
            player.Taskbar.Queue = new TaskbarQueueContainerComponent(MaxTaskbarQueue);

            foreach (DbShortcut dbShortcut in shortcuts)
            {
                Shortcut shortcut = null;

                if (dbShortcut.Type == ShortcutType.Item)
                {
                    Item item = player.Inventory.GetItemAtSlot(dbShortcut.ObjectItemSlot.GetValueOrDefault(-1));
                    
                    if (item is null)
                    {
                        return;
                    }

                    shortcut = new Shortcut(dbShortcut.Slot, dbShortcut.Type, item.Index, dbShortcut.ObjectType, dbShortcut.ObjectIndex, dbShortcut.UserId, dbShortcut.ObjectData, dbShortcut.Text);
                }
                else
                {
                    shortcut = new Shortcut(dbShortcut.Slot, dbShortcut.Type, dbShortcut.ObjectItemSlot, dbShortcut.ObjectType, dbShortcut.ObjectIndex, dbShortcut.UserId, dbShortcut.ObjectData, dbShortcut.Text);
                }

                if (dbShortcut.TargetTaskbar == ShortcutTaskbarTarget.Applet)
                {
                    AddShortcutToAppletTaskbar(player, shortcut);
                }
                else if (dbShortcut.TargetTaskbar == ShortcutTaskbarTarget.Item)
                {
                    AddShortcutToItemTaskbar(player, shortcut, dbShortcut.SlotLevelIndex);
                }
            }
        }

        /// <inheritdoc />
        public void Save(IPlayerEntity player)
        {
            DbCharacter character = _database.Characters
                .Include(x => x.TaskbarShortcuts)
                .FirstOrDefault(x => x.Id == player.PlayerData.Id);

            character.TaskbarShortcuts.Clear();

            _database.SaveChanges();

            foreach (Shortcut appletShortcut in player.Taskbar.Applets.Objects)
            {
                if (appletShortcut == null)
                {
                    continue;
                }

                var dbApplet = new DbShortcut(ShortcutTaskbarTarget.Applet, 
                    appletShortcut.Slot, 
                    appletShortcut.Type,
                    appletShortcut.ItemIndex, 
                    appletShortcut.ObjectType, 
                    appletShortcut.ObjectIndex, 
                    appletShortcut.UserId, 
                    appletShortcut.ObjectData, 
                    appletShortcut.Text);

                dbApplet.CharacterId = character.Id;

                if (appletShortcut.Type == ShortcutType.Item && appletShortcut.ItemIndex.HasValue)
                {
                    Item inventoryItem = player.Inventory.GetItemAtIndex(appletShortcut.ItemIndex.Value);
                    
                    if (inventoryItem is null)
                    {
                        continue;
                    }

                    dbApplet.ObjectItemSlot = inventoryItem.Slot;
                }

                _database.TaskbarShortcuts.Add(dbApplet);
            }


            for (int slotLevel = 0; slotLevel < player.Taskbar.Items.Objects.Count; slotLevel++)
            {
                for (int slot = 0; slot < player.Taskbar.Items.Objects[slotLevel].Count; slot++)
                {
                    Shortcut itemShortcut = player.Taskbar.Items.Objects[slotLevel][slot];
                    
                    if (itemShortcut == null)
                    {
                        continue;
                    }

                    var dbShortcut = new DbShortcut(ShortcutTaskbarTarget.Item, slotLevel, itemShortcut.Slot,
                        itemShortcut.Type, itemShortcut.ItemIndex, itemShortcut.ObjectType, itemShortcut.ObjectIndex,
                        itemShortcut.UserId, itemShortcut.ObjectData, itemShortcut.Text);

                    dbShortcut.CharacterId = character.Id;

                    if (itemShortcut.Type == ShortcutType.Item && itemShortcut.ItemIndex.HasValue)
                    {
                        Item item = player.Inventory.GetItemAtIndex(itemShortcut.ItemIndex.Value);
                        
                        if (item is null)
                        {
                            continue;
                        }

                        dbShortcut.ObjectItemSlot = item.Slot;
                    }

                    _database.TaskbarShortcuts.Add(dbShortcut);
                }
            }

            _database.SaveChanges();
        }

        /// <inheritdoc />
        public void AddShortcutToAppletTaskbar(IPlayerEntity player, Shortcut shortcutToAdd)
        {
            if (shortcutToAdd.Slot < 0 || shortcutToAdd.Slot >= MaxTaskbarApplets)
            {
                return;
            }

            player.Taskbar.Applets.Objects[shortcutToAdd.Slot] = shortcutToAdd;

            _logger.LogDebug("Created Shortcut of type {0} on slot {1} for player {2} on the Applet Taskbar", Enum.GetName(typeof(ShortcutType), shortcutToAdd.Type), shortcutToAdd.Slot, player.Object.Name);
        }

        /// <inheritdoc />
        public void AddShortcutToItemTaskbar(IPlayerEntity player, Shortcut shortcutToAdd, int slotLevelIndex)
        {
            if (slotLevelIndex < 0 || slotLevelIndex >= MaxTaskbarItemLevels)
            {
                return;
            }

            if (shortcutToAdd.Slot < 0 || shortcutToAdd.Slot >= MaxTaskbarItems)
            {
                return;
            }

            player.Taskbar.Items.Objects[slotLevelIndex][shortcutToAdd.Slot] = shortcutToAdd;

            _logger.LogDebug("Created Shortcut of type {0} on slot {1} for player {2} on the Item Taskbar", Enum.GetName(typeof(ShortcutType), shortcutToAdd.Type), shortcutToAdd.Slot, player.Object.Name);
        }

        /// <inheritdoc />
        public void RemoveShortcutFromAppletTaskbar(IPlayerEntity player, int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= MaxTaskbarApplets)
            {
                return;
            }

            player.Taskbar.Applets.Objects[slotIndex] = null;

            _logger.LogDebug("Removed Shortcut on slot {0} of player {1} on the Applet Taskbar", slotIndex, player.Object.Name);
        }

        /// <inheritdoc />
        public void RemoveShortcutFromItemTaskbar(IPlayerEntity player, int slotLevelIndex, int slotIndex)
        {
            if (slotLevelIndex < 0 || slotLevelIndex >= MaxTaskbarItemLevels)
            {
                return;
            }

            if (slotIndex < 0 || slotIndex >= MaxTaskbarItems)
            {
                return;
            }

            player.Taskbar.Items.Objects[slotLevelIndex][slotIndex] = null;

            _logger.LogDebug($"Removed Shortcut on slot {slotLevelIndex}-{slotIndex} of player {player.Object.Name} on the Item Taskbar");
        }
    }
}