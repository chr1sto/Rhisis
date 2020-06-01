using Rhisis.Core.Extensions;
using Rhisis.Network.Packets;
using Rhisis.World.Game.Structures;
using Sylver.Network.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Components
{
    public class ItemContainerComponent : IPacketSerializer, IEnumerable<Item>
    {
        protected const int Empty = -1;

        protected readonly List<Item> _items;
        protected readonly int[] _itemsMask;

        /// <summary>
        /// Gets the <see cref="ItemContainerComponent"/> max capacity.
        /// </summary>
        public int MaxCapacity { get; }

        /// <summary>
        /// Gets the <see cref="ItemContainerComponent"/> max storage capacity.
        /// </summary>
        public int MaxStorageCapacity { get; }

        /// <summary>
        /// Gets the extra storage capacity.
        /// </summary>
        public int ExtraCapacity { get; }

        /// <summary>
        /// Creates a new <see cref="ItemContainerComponent"/> instance.
        /// </summary>
        public ItemContainerComponent(int maxCapacity) :
            this(maxCapacity, maxCapacity)
        { }

        /// <summary>
        /// Creates a new <see cref="ItemContainerComponent"/> instance.
        /// </summary>
        public ItemContainerComponent(int maxCapacity, int maxStorageCapacity)
        {
            MaxCapacity = maxCapacity;
            MaxStorageCapacity = maxStorageCapacity;
            ExtraCapacity = MaxCapacity - MaxStorageCapacity;
            _itemsMask = new int[MaxCapacity];
            _items = Enumerable.Repeat((Item)null, MaxCapacity).ToList();

            for (int i = 0; i < MaxCapacity; i++)
            {
                _items[i] = new Item(Empty)
                {
                    Index = i
                };

                if (i < MaxStorageCapacity)
                {
                    _items[i].Slot = i;
                    _itemsMask[i] = i;
                }
                else
                {
                    _itemsMask[i] = Empty;
                }
            }
        }

        /// <summary>
        /// Initializes the item container.
        /// </summary>
        /// <param name="items">Items.</param>
        public void Initialize(IEnumerable<Item> items)
        {
            int itemIndex;
            int itemsCount = Math.Min(items.Count(), MaxCapacity);

            for (itemIndex = 0; itemIndex < itemsCount; itemIndex++)
            {
                Item item = items.ElementAtOrDefault(itemIndex);

                if (item != null)
                {
                    int slot = item.Slot;

                    _items[slot].CopyFrom(item);
                    _items[slot].Slot = slot;
                    _items[slot].Index = slot;
                    _itemsMask[slot] = slot;
                }
            }
        }

        /// <summary>
        /// Gets an item matching a given predicate.
        /// </summary>
        /// <param name="predicate">Match predicate.</param>
        /// <returns>Item matching the predicate function; null otherwise.</returns>
        public Item GetItem(Func<Item, bool> predicate)
        {
            Item item = _items.FirstOrDefault(predicate);

            if (item == null)
            {
                return null;
            }

            return item.Id != Empty ? item : null;
        }

        /// <summary>
        /// Gets an item by its id.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <returns>Item if found; null otherwise.</returns>
        public Item GetItemById(int id) => GetItem(x => x.Id == id);

        /// <summary>
        /// Gets the item at a given slot.
        /// </summary>
        /// <param name="slot">Container slot.</param>
        /// <returns></returns>
        public Item GetItemAtSlot(int slot)
        {
            if (slot < 0 || slot >= MaxCapacity)
            {
                throw new IndexOutOfRangeException();
            }

            int itemIndex = _itemsMask[slot];

            if (itemIndex < 0 || itemIndex >= MaxCapacity)
            {
                return null;
            }

            Item item = _items[itemIndex];

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), $"No item found at slot {slot}.");
            }

            return item.Id != Empty ? item : null;
        }

        /// <summary>
        /// Gets the item at a given index (or unique id).
        /// </summary>
        /// <param name="index">Index or Unique Id.</param>
        /// <returns></returns>
        public Item GetItemAtIndex(int index)
        {
            if (index < 0 || index >= MaxCapacity)
            {
                return null;
            }

            Item item = _items[index];

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), $"No item found at index {index}.");
            }

            return item.Id != Empty ? item : null;
        }

        /// <summary>
        /// Gets the number of items of the inventory.
        /// </summary>
        /// <returns></returns>
        public int GetItemCount() => _items.Count(x => x.Id != Empty);

        /// <summary>
        /// Gts the number of items in the inventory storage area.
        /// </summary>
        /// <returns></returns>
        public int GetItemCountInStorage() => _items.GetRange(0, MaxStorageCapacity).Count(x => x.Id != Empty);

        /// <summary>
        /// Check if there is available slots in the container.
        /// </summary>
        /// <returns>True if there is available slots; false otherwise.</returns>
        [Obsolete]
        public bool HasAvailableSlots() => GetAvailableSlot() != Empty;

        /// <summary>
        /// Check if the item container can store the given item.
        /// </summary>
        /// <param name="itemToStore">Item to store.</param>
        /// <returns>True if the item container can store the item; false otherwise.</returns>
        public bool CanStoreItem(Item itemToStore)
        {
            if (itemToStore == null)
            {
                return false;
            }

            int quantityToStore = itemToStore.Quantity;
            int itemToStoreMaxQuantity = itemToStore.Data.PackMax;

            for (int i = 0; i < MaxStorageCapacity; i++)
            {
                Item item = GetItemAtSlot(i);

                if (item == null)
                {
                    if (quantityToStore > itemToStoreMaxQuantity)
                    {
                        quantityToStore -= itemToStoreMaxQuantity;
                    }
                    else
                    {
                        return true;
                    }
                }
                else if (item.Id == itemToStore.Id)
                {
                    if (item.Quantity + quantityToStore > itemToStoreMaxQuantity)
                    {
                        quantityToStore -= itemToStoreMaxQuantity - item.Quantity;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Get an available slot number.
        /// </summary>
        /// <returns>Slot number if available; <see cref="Empty"/> otherwise.</returns>
        public int GetAvailableSlot()
        {
            for (int i = 0; i < MaxStorageCapacity; i++)
            {
                if (_itemsMask[i] == Empty || _items[_itemsMask[i]].Id == Empty)
                {
                    return i;
                }
            }

            return Empty;
        }

        /// <summary>
        /// Checks if the given slot is available.
        /// </summary>
        /// <param name="slot">Slot.</param>
        /// <returns>True if the slot is available; false otherwise.</returns>
        public bool IsSlotAvailable(int slot) => GetItemAtSlot(slot) == null;

        /// <summary>
        /// Swap two slots.
        /// </summary>
        /// <param name="sourceSlot">Source slot.</param>
        /// <param name="destinationSlot">Destination slot.</param>
        public void Swap(int sourceSlot, int destinationSlot)
        {
            _itemsMask.Swap(sourceSlot, destinationSlot);

            int itemSourceIndex = _itemsMask[sourceSlot];
            int itemDestinationIndex = _itemsMask[destinationSlot];

            if (itemSourceIndex != Empty)
            {
                _items[itemSourceIndex].Slot = sourceSlot;
            }

            if (itemDestinationIndex != Empty)
            {
                _items[itemDestinationIndex].Slot = destinationSlot;
            }
        }

        /// <summary>
        /// Sets an item at a given index.
        /// </summary>
        /// <param name="item">Index to set.</param>
        /// <param name="index">Index.</param>
        public void SetItemAtIndex(Item item, int index)
        {
            if (item != null)
            {
                _items[index] = item;
                _items[index].Index = index;
            }
        }

        /// <summary>
        /// Sets an item at a given slot.
        /// </summary>
        /// <param name="item">Item to set.</param>
        /// <param name="slot">Slot.</param>
        public void SetItemAtSlot(Item item, int slot)
        {
            item.Slot = slot;

            SetItemAtIndex(item, _itemsMask[slot]);
        }

        /// <summary>
        /// Adds an item to the item container.
        /// </summary>
        /// <param name="itemToAdd">Item to add.</param>
        public IEnumerable<ItemCreationResult> AddItem(Item itemToAdd)
        {
            int quantity = itemToAdd.Quantity;
            var result = new List<ItemCreationResult>();

            if (!CanStoreItem(itemToAdd))
            {
                return result;
            }

            if (itemToAdd.Data.IsStackable)
            {
                for (int i = 0; i < MaxStorageCapacity; i++)
                {
                    int itemIndex = _itemsMask[i];

                    if (itemIndex < 0 || itemIndex >= MaxCapacity)
                    {
                        continue;
                    }

                    Item inventoryItem = _items[itemIndex];

                    if (inventoryItem.Id == itemToAdd.Id && inventoryItem.Quantity < inventoryItem.Data.PackMax)
                    {
                        if (inventoryItem.Quantity + quantity > inventoryItem.Data.PackMax)
                        {
                            quantity -= inventoryItem.Data.PackMax - inventoryItem.Quantity;
                            inventoryItem.Quantity = inventoryItem.Data.PackMax;
                        }
                        else
                        {
                            inventoryItem.Quantity += quantity;
                            quantity = 0;
                        }

                        result.Add(new ItemCreationResult(ItemCreationActionType.Update, inventoryItem));

                        if (quantity == 0)
                        {
                            break;
                        }
                    }
                }
            }

            if (quantity > 0)
            {
                for (int i = 0; i < MaxStorageCapacity; i++)
                {
                    int itemIndex = _itemsMask[i];

                    if (itemIndex < 0 || itemIndex >= MaxCapacity)
                    {
                        continue;
                    }

                    Item inventoryItem = _items[itemIndex];

                    if (inventoryItem.Id == Empty)
                    {
                        inventoryItem.CopyFrom(itemToAdd);
                        inventoryItem.Index = itemIndex;
                        inventoryItem.Slot = i;

                        if (quantity > inventoryItem.Data.PackMax)
                        {
                            inventoryItem.Quantity = inventoryItem.Data.PackMax;
                            quantity -= inventoryItem.Quantity;
                        }
                        else
                        {
                            inventoryItem.Quantity = quantity;
                            quantity = 0;
                        }

                        result.Add(new ItemCreationResult(ItemCreationActionType.Add, inventoryItem));

                        if (quantity == 0)
                        {
                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Remove the given item from the item container.
        /// </summary>
        /// <param name="itemToRemove">Item to remove.</param>
        public void RemoveItem(Item itemToRemove)
        {
            if (itemToRemove == null || itemToRemove.Slot >= MaxCapacity)
            {
                return;
            }

            itemToRemove.Reset();

            if (itemToRemove.Slot >= MaxStorageCapacity)
            {
                _itemsMask[itemToRemove.Slot] = Empty;
                itemToRemove.Slot = Empty;
            }
        }

        /// <inheritdoc />
        public void Serialize(INetPacketStream packet)
        {
            for (int i = 0; i < MaxCapacity; i++)
            {
                packet.Write(_itemsMask[i]);
            }

            packet.Write((byte)GetItemCount());

            for (int i = 0; i < MaxCapacity; i++)
            {
                Item item = _items.ElementAt(i);

                if (item.Id != Empty)
                {
                    packet.Write((byte)i);
                    item.Serialize(packet, item.Index);
                }
            }

            for (int i = 0; i < MaxCapacity; i++)
            {
                packet.Write(_items[i].Slot);
            }
        }

        /// <inheritdoc />
        public IEnumerator<Item> GetEnumerator() => _items.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
