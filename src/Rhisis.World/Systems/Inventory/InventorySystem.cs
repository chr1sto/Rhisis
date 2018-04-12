using Rhisis.Core.Data;
using Rhisis.Core.Extensions;
using Rhisis.Core.IO;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Core.Systems;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rhisis.World.Systems.Inventory
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
        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            if (!(entity is IPlayerEntity playerEntity) || !(e is InventoryEventArgs inventoryEvent))
                return;
            
            if (!inventoryEvent.CheckArguments())
            {
                Logger.Error("Cannot execute inventory action: {0} due to invalid arguments.", inventoryEvent.ActionType);
                return;
            }
            
            Logger.Debug("Execute inventory action: {0}", inventoryEvent.ActionType.ToString());

            switch (inventoryEvent.ActionType)
            {
                case InventoryActionType.Initialize:
                    this.InitializeInventory(playerEntity, inventoryEvent);
                    break;
                case InventoryActionType.MoveItem:
                    this.MoveItem(playerEntity, inventoryEvent);
                    break;
                case InventoryActionType.Equip:
                    this.EquipItem(playerEntity, inventoryEvent);
                    break;
                case InventoryActionType.CreateItem:
                    this.CreateItem(playerEntity, inventoryEvent as InventoryCreateItemEventArgs);
                    break;
                case InventoryActionType.Unknown:
                    // Nothing to do.
                    break;
                default:
                    Logger.Warning("Unknown inventory action type: {0} for player {1} ", inventoryEvent.ActionType.ToString(), entity.Object.Name);
                    break;
            }
        }

        /// <summary>
        /// Initialize the player's inventory.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="args">Command arguments</param>
        private void InitializeInventory(IPlayerEntity player, InventoryEventArgs args)
        {
            Logger.Debug("Initialize inventory");

            if (args.ArgumentCount < 0)
                throw new ArgumentException("Inventory event arguments cannot be empty.", nameof(args));

            player.Inventory = new ItemContainerComponent(MaxItems, InventorySize);
            var inventory = player.Inventory;
            var dbItems = args.GetArgument<IEnumerable<Database.Structures.Item>>(0);

            if (dbItems != null)
            {
                foreach (Database.Structures.Item item in dbItems)
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

            Logger.Debug("Initialize inventory done");
        }

        /// <summary>
        /// Move an item.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="args"></param>
        private void MoveItem(IPlayerEntity player, InventoryEventArgs args)
        {
            if (args.ArgumentCount < 1)
                throw new ArgumentException("Inventory event arguments should be equal to 2.", nameof(args));

            var sourceSlot = args.GetArgument<byte>(0);
            var destinationSlot = args.GetArgument<byte>(1);
            List<Item> items = player.Inventory.Items;

            if (sourceSlot >= MaxItems || destinationSlot >= MaxItems)
                return;

            if (items[sourceSlot].Id == -1 || items[sourceSlot].UniqueId == -1 || items[destinationSlot].UniqueId == -1)
                return;
            
            Item sourceItem = items[sourceSlot];
            Item destItem = items[destinationSlot];
            
            Logger.Debug("Moving item from {0} to {1}", sourceSlot, destinationSlot);

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
        /// <param name="args"></param>
        private void EquipItem(IPlayerEntity player, InventoryEventArgs args)
        {
            if (args.ArgumentCount < 1)
                throw new ArgumentException("Inventory event arguments should be equal to 2.", nameof(args));

            var uniqueId = args.GetArgument<int>(0);
            var part = args.GetArgument<int>(1);

            Logger.Debug("UniqueId: {0} | Part: {1}", uniqueId, part);

            if (part >= MaxHumanParts)
            {
                Logger.Error("Cannot equip item with unique id: {0}. Not an equipable item.", uniqueId);
                return;
            }

            Item item = player.Inventory.GetItem(uniqueId);

            if (item == null)
                return;

            bool equip = part == -1;

            Logger.Debug("Equip item: {0}", equip.ToString());

            if (equip)
            {
                Logger.Debug("Item: {0}", item.ToString());

                // TODO: check if the player fits the item requirements
                if (item.Data.ItemKind1 == ItemKind1.ARMOR && item.Data.ItemSex != player.VisualAppearance.Gender)
                {
                    Logger.Debug("Wrong sex for armor");
                    // TODO: Send invalid sex error
                    return;
                }

                if (player.Object.Level < item.Data.LimitLevel)
                {
                    Logger.Debug("Player level to low");
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
            Logger.Debug("Available slot: {0}", availableSlot);

            if (availableSlot < 0)
            {
                Logger.Debug("No available slots.");
                WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LACKSPACE);
                return;
            }

            if (item.Id > 0 && item.Slot > EquipOffset)
            {
                int parts = Math.Abs(sourceSlot - EquipOffset);

                item.Slot = availableSlot;
                player.Inventory.Items.Swap(sourceSlot, availableSlot);

                WorldPacketFactory.SendItemEquip(player, item, parts, false);
            }
        }

        private void CreateItem(IPlayerEntity player, InventoryCreateItemEventArgs eventArgs)
        {
            if (eventArgs == null)
                throw new ArgumentNullException(nameof(eventArgs));

            ItemData itemData = eventArgs.ItemData;

            if (itemData.IsStackable)
            {
                // TODO: stackable items
            }
            else
            {
                for (var i = 0; i < eventArgs.Quantity; i++)
                {
                    int availableSlot = player.Inventory.GetAvailableSlot();

                    if (availableSlot < 0)
                    {
                        WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LACKSPACE);
                        break;
                    }

                    Logger.Debug("Available slot: {0}", availableSlot);

                    var newItem = new Item(eventArgs.ItemId, 1, eventArgs.CreatorId)
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
