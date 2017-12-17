using Ether.Network.Packets;
using Rhisis.World.Core.Systems;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rhisis.World.Systems
{
    [System]
    public class InventorySystem : NotifiableSystemBase
    {
        public const int RightWeaponSlot = 52;
        public const int EquipOffset = 42;
        public const int MaxItems = 73;
        public const int InventorySize = EquipOffset;
        public const int MaxHumanParts = MaxItems - EquipOffset;

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
        }

        /// <summary>
        /// Initialize the player's inventory.
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="dbItems">Player's item stored in database</param>
        public static void InitializeInventory(IPlayerEntity player, IEnumerable<Database.Structures.Item> dbItems)
        {
            InventoryComponent inventory = player.InventoryComponent;

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
        }

        public static void SerializeVisibleEffects(IPlayerEntity player, NetPacketBase packet)
        {
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
        }

        public static void SerializeEquipedItems(IPlayerEntity player, NetPacketBase packet)
        {
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
        }

        public static void SerializeInventory(IPlayerEntity player, NetPacketBase packet)
        {
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
        }
    }
}
