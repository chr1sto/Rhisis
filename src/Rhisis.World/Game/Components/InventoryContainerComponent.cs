using Rhisis.Core.Data;
using Rhisis.Core.Extensions;
using Rhisis.Core.IO;
using Rhisis.World.Game.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Rhisis.World.Game.Components
{
    public class InventoryContainerComponent : ItemContainerComponent
    {
        public const int EquipOffset = 42;
        public const int MaxInventoryItems = 73;
        public const int InventorySize = EquipOffset;
        public const int MaxHumanParts = MaxInventoryItems - EquipOffset;
        private const int MaxItemCoolTimes = 3;

        private readonly long[] _itemsCoolTimes = new long[MaxItemCoolTimes];

        /// <summary>
        /// Gets or sets the delayed action id of the current item behing used.
        /// </summary>
        public Guid ItemInUseActionId { get; set; }

        /// <summary>
        /// Creates a new <see cref="InventoryContainerComponent"/> instance.
        /// </summary>
        public InventoryContainerComponent()
            : base(MaxInventoryItems, InventorySize)
        {
        }

        /// <summary>
        /// Gets the equiped items.
        /// </summary>
        /// <returns>Collection of the equiped items.</returns>
        public IEnumerable<Item> GetEquipedItems()
        {
            return _itemsMask.GetRange(EquipOffset, MaxHumanParts).Select(x => _items.ElementAtOrDefault(x));
        }

        /// <summary>
        /// Gets an equiped item based on a given item part.
        /// </summary>
        /// <param name="equipedItemPart">Item part type.</param>
        /// <returns>Equiped item if there is one; null otherwise.</returns>
        public Item GetEquipedItem(ItemPartType equipedItemPart)
        {
            int equipedItemSlot = EquipOffset + (int)equipedItemPart;

            if (equipedItemSlot > MaxInventoryItems || equipedItemSlot < EquipOffset)
            {
                return null;
            }

            return GetItemAtSlot(equipedItemSlot);
        }

        /// <summary>
        /// Equips an item to the given destination part.
        /// </summary>
        /// <param name="itemToEquip">Item to equip.</param>
        /// <param name="destinationPart">Destination part.</param>
        /// <returns>True if the item has been equiped; false otherwise.</returns>
        public bool Equip(Item itemToEquip, ItemPartType destinationPart)
        {
            int sourceSlot = itemToEquip.Slot;
            int destinationSlot = MaxStorageCapacity + (int)destinationPart;

            if (itemToEquip.Slot == destinationSlot || itemToEquip.Slot >= MaxCapacity || destinationSlot >= MaxCapacity)
            {
                return false;
            }

            for (int i = 0; i < MaxCapacity; i++)
            {
                if (_items[i].Id == Empty && _items[i].Slot == Empty)
                {
                    _itemsMask[destinationSlot] = _itemsMask[sourceSlot];
                    _itemsMask[sourceSlot] = i;

                    _items[_itemsMask[sourceSlot]].Slot = sourceSlot;
                    _items[_itemsMask[destinationSlot]].Slot = destinationSlot;

                    return true;
                }
            }

            return true;
        }

        /// <summary>
        /// Unequip an item.
        /// </summary>
        /// <param name="itemToUnequip">Item to unequip.</param>
        /// <returns>True if the item has been unequiped; false otherwise.</returns>
        public bool Unequip(Item itemToUnequip)
        {
            int slot = itemToUnequip.Slot;

            if (slot >= MaxCapacity)
            {
                return false;
            }

            int itemIndex = _itemsMask[slot];

            if (itemIndex < 0 || itemIndex >= MaxCapacity)
            {
                return false;
            }

            for (int i = 0; i < MaxStorageCapacity; i++)
            {
                int emptyItemIndex = _itemsMask[i];

                if (emptyItemIndex < 0 || emptyItemIndex >= MaxCapacity)
                {
                    return false;
                }

                if (_items[emptyItemIndex].Id == Empty)
                {
                    _items[emptyItemIndex].Slot = Empty;
                    _itemsMask[slot] = Empty;

                    _items[itemIndex].Slot = i;
                    _itemsMask[i] = itemIndex;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the given item is equiped.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>True if the item is equiped; false otherwise.</returns>
        public bool IsItemEquiped(Item item) => item.Slot > EquipOffset;

        /// <summary>
        /// Gets the item cool time group.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <returns>Returns the item cool time group.</returns>
        public int? GetItemCoolTimeGroup(Item item)
        {
            if (item.Data.CoolTime <= 0)
            {
                return null;
            }

            return item.Data.ItemKind2 switch
            {
                ItemKind2.FOOD => item.Data.ItemKind3 == ItemKind3.PILL ? 1 : 0,
                ItemKind2.SKILL => 2,
                _ => null,
            };
        }

        /// <summary>
        /// Checks if the item has a cooltime.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ItemHasCoolTime(Item item) => GetItemCoolTimeGroup(item).HasValue;

        /// <summary>
        /// Check if the given item is a cooltime item and can be used.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <returns>Returns true if the item with cooltime can be used; false otherwise.</returns>
        public bool CanUseItemWithCoolTime(Item item)
        {
            int? group = GetItemCoolTimeGroup(item);

            return group.HasValue && _itemsCoolTimes[group.Value] < Time.GetElapsedTime();
        }

        /// <summary>
        /// Sets item cool time.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="cooltime">Cooltime.</param>
        public void SetCoolTime(Item item, int cooltime)
        {
            int? group = GetItemCoolTimeGroup(item);

            if (group.HasValue)
            {
                _itemsCoolTimes[group.Value] = Time.GetElapsedTime() + cooltime;
            }
        }
    }
}
