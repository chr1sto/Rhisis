using Ether.Network.Packets;
using Rhisis.Core.Data;
using Rhisis.Core.Extensions;
using Rhisis.Core.IO;
using Rhisis.World.Core.Systems;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rhisis.World.Systems
{
    [System]
    public class InventorySystem : NotifiableSystemBase
    {
        public static readonly int RightWeaponSlot = 52;
        public static readonly int EquipOffset = 42;
        public static readonly int MaxItems = 73;
        public static readonly int InventorySize = EquipOffset;
        public static readonly int MaxHumanParts = MaxItems - EquipOffset;

        /// <summary>
        /// Gets the <see cref="InventorySystem"/> match filter.
        /// </summary>
        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.Player;

        /// <summary>
        /// Creates a new <see cref="InventorySystem"/> instance.
        /// </summary>
        /// <param name="context"></param>
        public InventorySystem(IContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Executes the <see cref="InventorySystem"/> logic.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        public override void Execute(IEntity entity, EventArgs e)
        {
            if (e is InventoryEventArgs inventoryEvent)
            {
                var playerEntity = entity as IPlayerEntity;

                Logger.Debug("Execute inventory action: {0}", inventoryEvent.ActionType.ToString());

                switch (inventoryEvent.ActionType)
                {
                    case InventoryActionType.Initialize:
                        this.InitializeInventory(playerEntity, inventoryEvent.Arguments);
                        break;
                    case InventoryActionType.SerializeInventory:
                        this.SerializeInventory(playerEntity, inventoryEvent.Arguments);
                        break;
                    case InventoryActionType.SerializeEquipement:
                        this.SerializeEquipedItems(playerEntity, inventoryEvent.Arguments);
                        break;
                    case InventoryActionType.SerializeVisibleEffects:
                        this.SerializeVisibleEffects(playerEntity, inventoryEvent.Arguments);
                        break;
                    case InventoryActionType.MoveItem:
                        this.MoveItem(playerEntity, inventoryEvent.Arguments);
                        break;
                    case InventoryActionType.Equip:
                        this.EquipItem(playerEntity, inventoryEvent.Arguments);
                        break;
                }
            }
        }

        /// <summary>
        /// Initialize the player's inventory.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="dbItems">Player's item stored in database</param>
        private void InitializeInventory(IPlayerEntity player, object[] args)
        {
            Logger.Debug("Initialize inventory");

            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Length < 0)
                throw new ArgumentException("Inventory event arguments cannot be empty.", nameof(args));

            var dbItems = args[0] as IEnumerable<Database.Structures.Item>;
            ItemContainerComponent inventory = player.InventoryComponent;

            for (int i = 0; i < MaxItems; ++i)
            {
                inventory.Items[i] = new Item
                {
                    UniqueId = i
                };
            }

            if (dbItems != null && dbItems.Count() > 0)
            {
                foreach (var item in dbItems)
                {
                    int uniqueId = inventory.Items[item.ItemSlot].UniqueId;

                    inventory.Items[item.ItemSlot] = new Item(item.ItemId, item.ItemCount, item.CreatorId, item.ItemSlot, uniqueId, item.Refine, item.Element, item.ElementRefine);
                }
            }

            for (int i = EquipOffset; i < MaxItems; ++i)
            {
                if (inventory.Items[i].Id == -1)
                    inventory.Items[i].UniqueId = -1;
            }

            Logger.Debug("Initialize inventory done");
        }

        /// <summary>
        /// Serialize the inventory visible effects.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="args"></param>
        private void SerializeVisibleEffects(IPlayerEntity player, object[] args)
        {
            Logger.Debug("Serialize visible effects");

            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Length < 0)
                throw new ArgumentException("Inventory event arguments cannot be empty.", nameof(args));

            var packet = args[0] as NetPacketBase;
            IEnumerable<Item> equipedItems = player.InventoryComponent.Items.GetRange(EquipOffset, MaxItems - EquipOffset);

            foreach (var item in equipedItems)
            {
                if (item == null || item.Id < 0)
                    packet.Write(0);
                else
                {
                    packet.Write(item.Refine); // Refine
                    packet.Write<byte>(0);
                    packet.Write(item.Element); // element (fire, water, elec...)
                    packet.Write(item.ElementRefine); // Refine element
                }
            }

            Logger.Debug("Serialize visible effects done");
        }

        /// <summary>
        /// Serialize the inventory equiped items.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="args"></param>
        private void SerializeEquipedItems(IPlayerEntity player, object[] args)
        {
            Logger.Debug("Serialize equiped items");

            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Length < 0)
                throw new ArgumentException("Inventory event arguments cannot be empty.", nameof(args));

            var packet = args[0] as NetPacketBase;
            IEnumerable<Item> equipedItems = player.InventoryComponent.Items.GetRange(EquipOffset, MaxItems - EquipOffset);

            packet.Write((byte)equipedItems.Count(x => x.Id != -1));

            foreach (var item in equipedItems)
            {
                if (item != null && item.Id > 0)
                {
                    packet.Write((byte)(item.Slot - EquipOffset));
                    packet.Write((short)item.Id);
                    packet.Write<byte>(0);
                }
            }

            Logger.Debug("Serialize equiped items done");
        }

        /// <summary>
        /// Serialize player's inventory.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="args"></param>
        private void SerializeInventory(IPlayerEntity player, object[] args)
        {
            Logger.Debug("Serialize inventory");

            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Length < 0)
                throw new ArgumentException("Inventory event arguments cannot be empty.", nameof(args));

            var packet = args[0] as NetPacketBase;
            IList<Item> items = player.InventoryComponent.Items;

            for (int i = 0; i < MaxItems; ++i)
                packet.Write(items[i].UniqueId);

            packet.Write((byte)items.Count(x => x.Id != -1));

            for (int i = 0; i < MaxItems; ++i)
            {
                if (items[i].Id > 0)
                {
                    packet.Write((byte)items[i].UniqueId);
                    packet.Write(items[i].UniqueId);
                    items[i].Serialize(packet);
                }
            }

            for (int i = 0; i < MaxItems; ++i)
                packet.Write(items[i].UniqueId);

            Logger.Debug("Serialize inventory done");
        }

        /// <summary>
        /// Move an item.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="args"></param>
        private void MoveItem(IPlayerEntity player, object[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Length < 1)
                throw new ArgumentException("Inventory event arguments should be equal to 2.", nameof(args));

            int sourceSlot = Convert.ToInt32(args[0]);
            int destinationSlot = Convert.ToInt32(args[1]);
            var sourceItem = player.InventoryComponent.Items[sourceSlot];
            var destItem = player.InventoryComponent.Items[destinationSlot];
            
            Logger.Debug("Moving item from {0} to {1}", sourceSlot, destinationSlot);

            bool stackable = sourceItem.Id == destItem.Id && sourceItem.Data.PackMax > 1;

            if (stackable)
            {
                // TODO: stack items
            }
            else
            {
                sourceItem.Slot = destinationSlot;

                if (destItem.Slot != -1)
                    destItem.Slot = sourceSlot;

                player.InventoryComponent.Items.Swap(sourceSlot, destinationSlot);
                WorldPacketFactory.SendItemMove(player, (byte)sourceSlot, (byte)destinationSlot);
            }
        }

        /// <summary>
        /// Equips an item.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="args"></param>
        private void EquipItem(IPlayerEntity player, object[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (args.Length < 1)
                throw new ArgumentException("Inventory event arguments should be equal to 2.", nameof(args));

            int uniqueId = Convert.ToInt32(args[0]);
            int part = Convert.ToInt32(args[1]);

            Logger.Debug("UniqueId: {0} | Part: {1}", uniqueId, part);

            if (part >= MaxHumanParts)
            {
                Logger.Error("Cannot equip item with unique id: {0}. Not an equipable item.", uniqueId);
                return;
            }

            var item = player.InventoryComponent.GetItem(uniqueId);

            if (item == null)
                return;

            bool equip = part == -1;

            Logger.Debug("Equip item: {0}", equip.ToString());

            if (equip)
            {
                Logger.Debug("Item: {0}", item.ToString());

                // TODO: check if the player fits the item requirements
                if (item.Data.ItemKind1 == ItemKind1.ARMOR && item.Data.ItemSex != player.HumanComponent.Gender)
                {
                    Logger.Debug("Wrong sex for armor");
                    // TODO: Send invalid sex error
                    return;
                }

                if (player.ObjectComponent.Level < item.Data.LimitLevel)
                {
                    Logger.Debug("Player level to low");
                    // TODO: Send low level error
                    return;
                }

                // TODO: SPECIAL: double weapon for blades...

                int sourceSlot = item.Slot;
                int equipedItemSlot = item.Data.Parts + EquipOffset;
                var equipedItem = player.InventoryComponent.GetItemBySlot(equipedItemSlot);
                
                if (equipedItem != null && equipedItem.Slot != -1)
                {
                    this.UnequipItem(player, equipedItem);
                }

                // Move item
                item.Slot = equipedItemSlot;
                player.InventoryComponent.Items.Swap(sourceSlot, equipedItemSlot);

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
            int availableSlot = player.InventoryComponent.GetAvailableSlot();
            Logger.Debug("Available slot: {0}", availableSlot);

            if (availableSlot == -1)
            {
                Logger.Debug("No available slots.");
                // TODO: send error to client. No more space in inventory.
                return;
            }

            if (item.Id > 0 && item.Slot > EquipOffset)
            {
                int parts = Math.Abs(sourceSlot - EquipOffset);

                item.Slot = availableSlot;
                player.InventoryComponent.Items.Swap(sourceSlot, availableSlot);

                WorldPacketFactory.SendItemEquip(player, item, parts, false);
            }
        }
    }
}
