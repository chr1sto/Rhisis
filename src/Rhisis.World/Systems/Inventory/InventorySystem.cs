using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Game;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Factories;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Drop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Inventory
{
    [Injectable(ServiceLifetime.Transient)]
    public sealed class InventorySystem : IInventorySystem
    {
        public const int RightWeaponSlot = 52;
        public const int EquipOffset = 42;
        public const int MaxItems = 73;
        public const int InventorySize = EquipOffset;
        public const int MaxHumanParts = MaxItems - EquipOffset;
        public static readonly Item Hand = new Item(11, 1, -1, RightWeaponSlot);

        private readonly ILogger<InventorySystem> _logger;
        private readonly IDatabase _database;
        private readonly IItemFactory _itemFactory;
        private readonly IInventoryPacketFactory _inventoryPacketFactory;
        private readonly IInventoryItemUsage _inventoryItemUsage;
        private readonly IDropSystem _dropSystem;
        private readonly ITextPacketFactory _textPacketFactory;

        public int Order => 0;

        public InventorySystem(ILogger<InventorySystem> logger, IDatabase database, IItemFactory itemFactory, IInventoryPacketFactory inventoryPacketFactory, IInventoryItemUsage inventoryItemUsage, IDropSystem dropSystem, ITextPacketFactory textPacketFactory)
        {
            _logger = logger;
            _database = database;
            _itemFactory = itemFactory;
            _inventoryPacketFactory = inventoryPacketFactory;
            _inventoryItemUsage = inventoryItemUsage;
            _dropSystem = dropSystem;
            _textPacketFactory = textPacketFactory;
        }

        /// <inheritdoc />
        public void Initialize(IPlayerEntity player)
        {
            IEnumerable<DbItem> items = _database.Items.GetAll(x => x.CharacterId == player.PlayerData.Id && !x.IsDeleted);

            player.Inventory = new ItemContainerComponent(MaxItems, InventorySize);

            if (items == null)
                return;

            foreach (DbItem item in items)
            {
                int uniqueId = player.Inventory.Items[item.ItemSlot].UniqueId;

                player.Inventory.Items[item.ItemSlot] = _itemFactory.CreateItem(item);
                player.Inventory.Items[item.ItemSlot].UniqueId = uniqueId;
            }
        }

        /// <inheritdoc />
        public void Save(IPlayerEntity player)
        {
            DbCharacter character = _database.Characters.Get(player.PlayerData.Id);
            IEnumerable<DbItem> itemsToDelete = from dbItem in character.Items
                                                let inventoryItem = player.Inventory.GetItem(x => x.DbId == dbItem.Id)
                                                where !dbItem.IsDeleted && inventoryItem == null || (inventoryItem != null && inventoryItem.Id == -1)
                                                select dbItem;

            foreach (DbItem dbItem in itemsToDelete)
            {
                dbItem.IsDeleted = true;

                _database.Items.Update(dbItem);
            }

            // Add or update items
            foreach (var item in player.Inventory.Items)
            {
                if (item.Id == -1)
                {
                    continue;
                }

                DbItem dbItem = character.Items.FirstOrDefault(x => x.Id == item.DbId && !x.IsDeleted);

                if (dbItem != null && dbItem.Id != 0)
                {
                    dbItem.CharacterId = player.PlayerData.Id;
                    dbItem.ItemId = item.Id;
                    dbItem.ItemCount = item.Quantity;
                    dbItem.ItemSlot = item.Slot;
                    dbItem.Refine = item.Refine;
                    dbItem.Element = (byte)item.Element;
                    dbItem.ElementRefine = item.ElementRefine;

                    _database.Items.Update(dbItem);
                }
                else
                {
                    dbItem = new DbItem
                    {
                        CharacterId = player.PlayerData.Id,
                        CreatorId = item.CreatorId,
                        ItemId = item.Id,
                        ItemCount = item.Quantity,
                        ItemSlot = item.Slot,
                        Refine = item.Refine,
                        Element = (byte)item.Element,
                        ElementRefine = item.ElementRefine
                    };

                    _database.Items.Create(dbItem);
                }
            }

            _database.Complete();
        }

        /// <inheritdoc />
        public int CreateItem(IPlayerEntity player, ItemDescriptor item, int quantity, int creatorId = -1, bool sendToPlayer = true)
        {
            int createdAmount = 0;

            if (item.Data.IsStackable)
            {
                for (var i = 0; i < EquipOffset; i++)
                {
                    Item inventoryItem = player.Inventory.Items[i];

                    if (inventoryItem.Id == item.Id)
                    {
                        if (inventoryItem.Quantity + quantity > item.Data.PackMax)
                        {
                            int boughtQuantity = inventoryItem.Data.PackMax - inventoryItem.Quantity;

                            createdAmount = boughtQuantity;
                            quantity -= boughtQuantity;
                            inventoryItem.Quantity = inventoryItem.Data.PackMax;
                        }
                        else
                        {
                            createdAmount = quantity;
                            inventoryItem.Quantity += quantity;
                            quantity = 0;
                        }

                        if (sendToPlayer)
                            _inventoryPacketFactory.SendItemUpdate(player, UpdateItemType.UI_NUM, inventoryItem.UniqueId, inventoryItem.Quantity);
                    }
                }

                if (quantity > 0)
                {
                    if (!player.Inventory.HasAvailableSlots())
                    {
                        _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LACKSPACE);
                    }
                    else
                    {
                        int availableSlot = player.Inventory.GetAvailableSlot();

                        Item newItem = _itemFactory.CreateItem(item.Id, item.Refine, item.Element, item.ElementRefine, creatorId);

                        if (newItem == null)
                        {
                            throw new ArgumentNullException(nameof(newItem));
                        }

                        newItem.Quantity = quantity;
                        newItem.UniqueId = player.Inventory[availableSlot].UniqueId;
                        newItem.Slot = availableSlot;
                        player.Inventory[availableSlot] = newItem;

                        if (sendToPlayer)
                            _inventoryPacketFactory.SendItemCreation(player, newItem);

                        createdAmount += quantity;
                    }
                }
            }
            else
            {
                while (quantity > 0)
                {
                    if (!player.Inventory.HasAvailableSlots())
                    {
                        _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LACKSPACE);
                        break;
                    }

                    int availableSlot = player.Inventory.GetAvailableSlot();

                    Item newItem = _itemFactory.CreateItem(item.Id, item.Refine, item.Element, item.ElementRefine, creatorId);

                    if (newItem == null)
                    {
                        throw new ArgumentNullException(nameof(newItem));
                    }

                    newItem.Quantity = 1;
                    newItem.UniqueId = player.Inventory[availableSlot].UniqueId;
                    newItem.Slot = availableSlot;
                    player.Inventory[availableSlot] = newItem;

                    if (sendToPlayer)
                        _inventoryPacketFactory.SendItemCreation(player, newItem);

                    createdAmount++;
                    quantity--;
                }
            }

            return createdAmount;
        }

        /// <inheritdoc />
        public int DeleteItem(IPlayerEntity player, int itemUniqueId, int quantity, bool sendToPlayer = true)
        {
            if (quantity <= 0)
                return 0;

            Item itemToDelete = player.Inventory.GetItem(itemUniqueId);

            if (itemToDelete == null)
                throw new ArgumentNullException(nameof(itemToDelete), $"Cannot find item with unique id: '{itemUniqueId}' in '{player.Object.Name}''s inventory.");

            return DeleteItem(player, itemToDelete, quantity, sendToPlayer);
        }

        /// <inheritdoc />
        public int DeleteItem(IPlayerEntity player, Item itemToDelete, int quantity, bool sendToPlayer = true)
        {
            int quantityToDelete = Math.Min(itemToDelete.Quantity, quantity);

            itemToDelete.Quantity -= quantityToDelete;

            if (sendToPlayer)
                _inventoryPacketFactory.SendItemUpdate(player, UpdateItemType.UI_NUM, itemToDelete.UniqueId, itemToDelete.Quantity);

            if (itemToDelete.Quantity <= 0)
                itemToDelete.Reset();

            return quantityToDelete;
        }

        /// <inheritdoc />
        public void MoveItem(IPlayerEntity player, byte sourceSlot, byte destinationSlot, bool sendToPlayer = true)
        {
            if (sourceSlot >= MaxItems)
            {
                throw new InvalidOperationException("Source slot is out of inventory range.");
            }

            if (destinationSlot >= MaxItems)
            {
                throw new InvalidOperationException("Destination slot is out of inventory range.");
            }

            if (sourceSlot == destinationSlot)
            {
                // Nothing to do when moving an item to the same slot.
                return;
            }

            Item sourceItem = player.Inventory[sourceSlot];
            Item destinationItem = player.Inventory[destinationSlot];

            if (sourceItem.Id == destinationItem.Id && sourceItem.Data.IsStackable)
            {
                int newQuantity = sourceItem.Quantity + destinationItem.Quantity;

                if (newQuantity > destinationItem.Data.PackMax)
                {
                    destinationItem.Quantity = destinationItem.Data.PackMax;
                    sourceItem.Quantity = newQuantity - sourceItem.Data.PackMax;

                    _inventoryPacketFactory.SendItemUpdate(player, UpdateItemType.UI_NUM, sourceItem.UniqueId, sourceItem.Quantity);
                    _inventoryPacketFactory.SendItemUpdate(player, UpdateItemType.UI_NUM, destinationItem.UniqueId, destinationItem.Quantity);
                }
                else
                {
                    destinationItem.Quantity = newQuantity;
                    DeleteItem(player, sourceItem.UniqueId, sourceItem.Quantity);
                    _inventoryPacketFactory.SendItemUpdate(player, UpdateItemType.UI_NUM, destinationItem.UniqueId, destinationItem.Quantity);
                }
            }
            else
            {
                sourceItem.Slot = destinationSlot;

                if (destinationItem.Slot != -1)
                    destinationItem.Slot = sourceSlot;

                player.Inventory.Items.Swap(sourceSlot, destinationSlot);

                if (sendToPlayer)
                    _inventoryPacketFactory.SendItemMove(player, sourceSlot, destinationSlot);
            }
        }

        /// <inheritdoc />
        public void EquipItem(IPlayerEntity player, int itemUniqueId, int equipPart)
        {
            Item itemToEquip = player.Inventory.GetItem(itemUniqueId);

            if (itemToEquip == null)
            {
                throw new ArgumentNullException(nameof(itemToEquip), $"Cannot find item with unique id: '{itemUniqueId}' in {player.Object.Name} inventory.");
            }

            if (!player.Inventory.HasAvailableSlots())
            {
                _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LACKSPACE);
                return;
            }

            bool shouldEquip = !itemToEquip.IsEquipped();

            if (shouldEquip)
            {
                if (IsItemEquipable(player, itemToEquip))
                {
                    int sourceSlot = itemToEquip.Slot;
                    int equipSlot = itemToEquip.Data.Parts + EquipOffset;

                    MoveItem(player, (byte)sourceSlot, (byte)equipSlot, sendToPlayer: false);
                    MoveItem(player, (byte)sourceSlot, (byte)player.Inventory.GetAvailableSlot(), sendToPlayer: false);
                    _inventoryPacketFactory.SendItemEquip(player, itemToEquip, itemToEquip.Data.Parts, true);
                }
            }
            else
            {
                if (itemToEquip.IsEquipped())
                {
                    int targetPart = Math.Abs(itemToEquip.Slot - EquipOffset);

                    if (equipPart != targetPart)
                    {
                        throw new InvalidOperationException($"Equipement parts doesn't match.");
                    }

                    MoveItem(player, (byte)itemToEquip.Slot, (byte)player.Inventory.GetAvailableSlot(), sendToPlayer: false);
                    _inventoryPacketFactory.SendItemEquip(player, itemToEquip, targetPart, false);
                }
            }
        }

        /// <inheritdoc />
        public void UseItem(IPlayerEntity player, int itemUniqueId, int part)
        {
            Item itemToUse = player.Inventory.GetItem(itemUniqueId);

            if (itemToUse == null)
            {
                throw new ArgumentNullException(nameof(itemToUse), $"Cannot find item with unique id: '{itemUniqueId}' in {player.Object.Name} inventory.");
            }

            if (part != -1)
            {
                if (part >= MaxHumanParts)
                {
                    throw new InvalidOperationException($"Invalid equipement part.");
                }

                if (!player.Battle.IsFighting)
                {
                    EquipItem(player, itemUniqueId, part);
                }
            }
            else
            {
                if (itemToUse.Data.IsUseable && itemToUse.Quantity > 0)
                {
                    _logger.LogTrace($"{player.Object.Name} want to use {itemToUse.Data.Name}.");

                    if (player.Inventory.ItemHasCoolTime(itemToUse) && !player.Inventory.CanUseItemWithCoolTime(itemToUse))
                    {
                        _logger.LogDebug($"Player '{player.Object.Name}' cannot use item {itemToUse.Data.Name}: CoolTime.");
                        return;
                    }

                    switch (itemToUse.Data.ItemKind2)
                    {
                        case ItemKind2.REFRESHER:
                        case ItemKind2.POTION:
                        case ItemKind2.FOOD:
                            _inventoryItemUsage.UseFoodItem(player, itemToUse);
                            break;
                        case ItemKind2.MAGIC:
                            _inventoryItemUsage.UseMagicItem(player, itemToUse);
                            break;
                        case ItemKind2.SYSTEM:
                            UseSystemItem(player, itemToUse);
                            break;
                        case ItemKind2.GMTEXT:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                        case ItemKind2.GENERAL:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                        case ItemKind2.BUFF:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                        case ItemKind2.BUFF2:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                        case ItemKind2.AIRFUEL:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                        case ItemKind2.FURNITURE:
                        case ItemKind2.PAPERING:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                        case ItemKind2.GUILDHOUSE_FURNITURE:
                        case ItemKind2.GUILDHOUSE_NPC:
                        case ItemKind2.GUILDHOUSE_PAPERING:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                        case ItemKind2.GUILDHOUES_COMEBACK:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                        case ItemKind2.BLINKWING:
                            _inventoryItemUsage.UseBlinkwingItem(player, itemToUse);
                            break;
                        default:
                            _logger.LogDebug($"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            _textPacketFactory.SendSnoop(player, $"Item usage for {itemToUse.Data.ItemKind2} is not implemented.");
                            break;
                    }
                }
            }
        }

        public void UseSystemItem(IPlayerEntity player, Item systemItem)
        {
            switch (systemItem.Data.ItemKind3)
            {
                case ItemKind3.SCROLL:
                    UseScrollItem(player, systemItem);
                    break;
                default:
                    _logger.LogDebug($"Item usage for {systemItem.Data.ItemKind3} is not implemented.");
                    _textPacketFactory.SendSnoop(player, $"Item usage for {systemItem.Data.ItemKind3} is not implemented.");
                    break;
            }
        }

        public void UseScrollItem(IPlayerEntity player, Item scrollItem)
        {
            switch (scrollItem.Data.Id)
            {
                case DefineItem.II_SYS_SYS_SCR_PERIN:
                    _inventoryItemUsage.UsePerin(player, scrollItem);
                    break;
                default:
                    _logger.LogDebug($"Item usage for {scrollItem.Data.Id} is not implemented.");
                    _textPacketFactory.SendSnoop(player, $"Item usage for {scrollItem.Data.Id} is not implemented.");
                    break;
            }
        }

        /// <inhertidoc />
        public void DropItem(IPlayerEntity player, int itemUniqueId, int quantity)
        {
            Item itemToDrop = player.Inventory.GetItem(itemUniqueId);

            if (itemToDrop == null)
            {
                throw new ArgumentNullException(nameof(itemToDrop), $"Cannot find item with unique id: '{itemUniqueId}' in {player.Object.Name} inventory.");
            }

            if (itemToDrop.Slot >= EquipOffset)
            {
                throw new InvalidOperationException($"Cannot drop an equiped item.");
            }

            int quantityToDrop = Math.Min(quantity, itemToDrop.Quantity);

            if (quantityToDrop <= 0)
            {
                throw new InvalidOperationException("Cannot drop a zero or negative quantit.");
            }

            itemToDrop.Quantity = quantityToDrop;
            _dropSystem.DropItem(player, itemToDrop, owner: null);
            DeleteItem(player, itemUniqueId, quantityToDrop);
        }

        /// <summary>
        /// Check if the given item is equipable by a player.
        /// </summary>
        /// <param name="player">Player trying to equip an item.</param>
        /// <param name="item">Item to equip.</param>
        /// <returns>True if the player can equip the item; false otherwise.</returns>
        private bool IsItemEquipable(IPlayerEntity player, Item item)
        {
            if (item.Data.ItemSex != int.MaxValue && item.Data.ItemSex != player.VisualAppearance.Gender)
            {
                _logger.LogDebug("Wrong sex for armor");
                _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_WRONGSEX, item.Data.Name);
                return false;
            }

            if (player.Object.Level < item.Data.LimitLevel)
            {
                _logger.LogDebug("Player level to low");
                _textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_REQLEVEL, item.Data.LimitLevel.ToString());
                return false;
            }

            return true;
        }
    }
}
