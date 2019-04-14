using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Systems;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Inventory.EventArgs;
using System;
using System.Collections.Generic;

namespace Rhisis.World.Systems.Inventory
{
    [System(SystemType.Notifiable)]
    public class InventorySystem : ISystem
    {
        public static readonly int RightWeaponSlot = 52;
        public static readonly int EquipOffset = 42;
        public static readonly int MaxItems = 73;
        public static readonly int InventorySize = EquipOffset;
        public static readonly int MaxHumanParts = MaxItems - EquipOffset;
        public static readonly Item Hand = new Item(11, 1, -1, RightWeaponSlot);

        private readonly ILogger Logger;

        /// <inheritdoc />
        public WorldEntityType Type => WorldEntityType.Player;

        /// <summary>
        /// Creates a new <see cref="InventorySystem"/> instance.
        /// </summary>
        public InventorySystem()
        {
            this.Logger = DependencyContainer.Instance.Resolve<ILogger<InventorySystem>>();
        }

        /// <inheritdoc />
        public void Execute(IEntity entity, SystemEventArgs e)
        {
            if (!(entity is IPlayerEntity playerEntity))
                return;
            
            if (!e.CheckArguments())
            {
                Logger.LogError("Cannot execute inventory action: {0} due to invalid arguments.", e.GetType());
                return;
            }

            switch (e)
            {
                case InventoryInitializeEventArgs inventoryInitializeEvent:
                    this.InitializeInventory(playerEntity, inventoryInitializeEvent);
                    break;
                case InventoryMoveEventArgs inventoryMoveEvent:
                    this.MoveItem(playerEntity, inventoryMoveEvent);
                    break;
                case InventoryEquipEventArgs inventoryEquipEvent:
                    this.EquipItem(playerEntity, inventoryEquipEvent);
                    break;
                case InventoryCreateItemEventArgs inventoryCreateItemEventArgs:
                    this.CreateItem(playerEntity, inventoryCreateItemEventArgs);
                    break;
                default:
                    Logger.LogWarning("Unknown inventory action type: {0} for player {1}", e.GetType(), entity.Object.Name);
                    break;
            }
        }

        /// <summary>
        /// Initialize the player's inventory.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="e"></param>
        private void InitializeInventory(IPlayerEntity player, InventoryInitializeEventArgs e)
        {
            player.Inventory = new ItemContainerComponent(MaxItems, InventorySize);
            var inventory = player.Inventory;

            if (e.Items != null)
            {
                foreach (Database.Entities.DbItem item in e.Items)
                {
                    int uniqueId = inventory.Items[item.ItemSlot].UniqueId;

                    inventory.Items[item.ItemSlot] = new Item(item)
                    {
                        UniqueId = uniqueId,
                    };
                }
            }

            for (int i = EquipOffset; i < MaxItems; ++i)
            {
                if (inventory.Items[i].Id == -1)
                    inventory.Items[i].UniqueId = -1;
            }
        }

        /// <summary>
        /// Move an item.
        /// </summary>
        /// <param name="player"></param>
        private void MoveItem(IPlayerEntity player, InventoryMoveEventArgs e)
        {
            var sourceSlot = e.SourceSlot;
            var destinationSlot = e.DestinationSlot;
            List<Item> items = player.Inventory.Items;

            if (sourceSlot >= MaxItems || destinationSlot >= MaxItems)
                return;

            if (items[sourceSlot].Id == -1 || items[sourceSlot].UniqueId == -1 || items[destinationSlot].UniqueId == -1)
                return;
            
            Item sourceItem = items[sourceSlot];
            Item destItem = items[destinationSlot];
            
            Logger.LogDebug("Moving item from {0} to {1}", sourceSlot, destinationSlot);

            if (sourceItem.Id == destItem.Id && sourceItem.Data.IsStackable)
            {
                // TODO: stack items
            }
            else
            {
                sourceItem.Slot = destinationSlot;

                if (destItem.Slot != -1)
                    destItem.Slot = sourceSlot;

                items.Swap(sourceSlot, destinationSlot);
                WorldPacketFactory.SendItemMove(player, sourceSlot, destinationSlot);
            }
        }

        /// <summary>
        /// Equips an item.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="e"></param>
        private void EquipItem(IPlayerEntity player, InventoryEquipEventArgs e)
        {
            var uniqueId = e.UniqueId;
            var part = e.Part;

            Logger.LogDebug("UniqueId: {0} | Part: {1}", uniqueId, part);

            Item item = player.Inventory.GetItem(uniqueId);
            if (item == null)
                return;

            bool equip = part == -1;

            Logger.LogDebug("Equip item: {0}", equip.ToString());

            if (equip)
            {
                Logger.LogDebug("Item: {0}", item.ToString());

                // TODO: check if the player fits the item requirements
                if (item.Data.ItemKind1 == ItemKind1.ARMOR && item.Data.ItemSex != player.VisualAppearance.Gender)
                {
                    Logger.LogDebug("Wrong sex for armor");
                    // TODO: Send invalid sex error
                    return;
                }

                if (player.Object.Level < item.Data.LimitLevel)
                {
                    Logger.LogDebug("Player level to low");
                    // TODO: Send low level error
                    return;
                }

                // TODO: SPECIAL: double weapon for blades...

                int sourceSlot = item.Slot;
                int equipedItemSlot = item.Data.Parts + EquipOffset;
                Item equipedItem = player.Inventory[equipedItemSlot];
                
                if (equipedItem != null && equipedItem.Slot != -1)
                {
                    this.UnequipItem(player, equipedItem);
                }

                // Move item
                item.Slot = equipedItemSlot;
                player.Inventory.Items.Swap(sourceSlot, equipedItemSlot);

                WorldPacketFactory.SendItemEquip(player, item, item.Data.Parts, true);
            }
            else
            {
                this.UnequipItem(player, item);
            }
        }

        /// <summary>
        /// Unequip an item.
        /// </summary>
        /// <param name="player">Player entity</param>
        /// <param name="item">Item to unequip</param>
        private void UnequipItem(IPlayerEntity player, Item item)
        {
            int sourceSlot = item.Slot;
            int availableSlot = player.Inventory.GetAvailableSlot();
            Logger.LogDebug("Available slot: {0}", availableSlot);

            if (availableSlot < 0)
            {
                Logger.LogDebug("No available slots.");
                WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LACKSPACE);
                return;
            }

            if (item.Id > 0 && item.IsEquipped())
            {
                int parts = Math.Abs(sourceSlot - EquipOffset);

                item.Slot = availableSlot;
                player.Inventory.Items.Swap(sourceSlot, availableSlot);

                WorldPacketFactory.SendItemEquip(player, item, parts, false);
            }
        }

        /// <summary>
        /// Create a new item.
        /// </summary>
        /// <param name="player">Player entity</param>
        /// <param name="e"></param>
        private void CreateItem(IPlayerEntity player, InventoryCreateItemEventArgs e)
        {
            ItemData itemData = e.ItemData;

            if (itemData.IsStackable)
            {
                int quantity = Math.Min(e.Quantity, itemData.PackMax);

                for (int i = 0; i < EquipOffset; i++)
                {
                    Item inventoryItem = player.Inventory.Items[i];

                    if (inventoryItem == null)
                        continue;

                    if (inventoryItem.Id == e.ItemId)
                    {
                        if (inventoryItem.Quantity + quantity > itemData.PackMax)
                        {
                            inventoryItem.Quantity = itemData.PackMax;
                            quantity -= itemData.PackMax - inventoryItem.Quantity;
                        }
                        else
                        {
                            inventoryItem.Quantity += quantity;
                            quantity = 0;
                        }

                        WorldPacketFactory.SendItemUpdate(player, UpdateItemType.UI_NUM, inventoryItem.UniqueId, inventoryItem.Quantity);
                    }
                }

                if (quantity > 0)
                {
                    if (!player.Inventory.HasAvailableSlots())
                        WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LACKSPACE);
                    else
                    {
                        var item = new Item(e.ItemId, quantity, -1);

                        if (player.Inventory.CreateItem(item))
                            WorldPacketFactory.SendItemCreation(player, item);
                        else
                        {
                            Logger.LogError("Inventory: Failed to create item.");
                        }
                    }
                }
            }
            else
            {
                for (var i = 0; i < e.Quantity; i++)
                {
                    int availableSlot = player.Inventory.GetAvailableSlot();

                    if (availableSlot < 0)
                    {
                        WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LACKSPACE);
                        break;
                    }

                    var newItem = new Item(e.ItemId, 1, e.CreatorId)
                    {
                        Slot = availableSlot,
                        UniqueId = availableSlot,
                    };

                    player.Inventory.Items[availableSlot] = newItem;
                    WorldPacketFactory.SendItemCreation(player, newItem);
                }
            }
        }
    }
}
