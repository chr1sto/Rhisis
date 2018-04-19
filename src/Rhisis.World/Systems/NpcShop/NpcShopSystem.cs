using Rhisis.Core.Data;
using Rhisis.Core.IO;
using Rhisis.Core.Structures.Game;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Inventory;
using System;
using System.Linq.Expressions;

namespace Rhisis.World.Systems.NpcShop
{
    [System]
    public class NpcShopSystem : NotifiableSystemBase
    {
        /// <inheritdoc />
        protected override Expression<Func<IEntity, bool>> Filter => x => x.Type == WorldEntityType.Player;

        /// <inheritdoc />
        public NpcShopSystem(IContext context) 
            : base(context)
        {
        }

        /// <inheritdoc />
        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            if (!(entity is IPlayerEntity player) || !(e is NpcShopEventArgs shopEvent))
            {
                Logger.Error("ShopSystem: Invalid event arguments.");
                return;
            }

            if (!shopEvent.CheckArguments())
            {
                Logger.Error("ShopSystem: Invalid event action arguments.");
                return;
            }

            switch (shopEvent.ActionType)
            {
                case NpcShopActionType.Open:
                    this.OpenShop(player, shopEvent);
                    break;

                case NpcShopActionType.Close:
                    this.CloseShop(player, shopEvent);
                    break;

                case NpcShopActionType.Buy:
                    this.BuyItem(player, shopEvent);
                    break;

                case NpcShopActionType.Sell:
                    this.SellItem(player, shopEvent);
                    break;

                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Opens the NPC Shop.
        /// </summary>
        /// <param name="player">Player entity</param>
        /// <param name="e"></param>
        private void OpenShop(IPlayerEntity player, NpcShopEventArgs e)
        {
            if (!(e is NpcShopOpenEventArgs npcEvent))
                throw new ArgumentException("ShopSystem: Invalid event arguments for OpenShop action.");

            var npc = player.Context.FindEntity<INpcEntity>(npcEvent.NpcObjectId);

            if (npc == null)
            {
                Logger.Error("ShopSystem: Cannot find NPC with object id : {0}", npcEvent.NpcObjectId);
                return;
            }

            if (npc.Shop == null)
            {
                Logger.Error("ShopSystem: NPC '{0}' doesn't have a shop.", npc.Object.Name);
                return;
            }
            
            player.PlayerData.CurrentShopName = npc.Object.Name;

            WorldPacketFactory.SendNpcShop(player, npc);
        }

        /// <summary>
        /// Closes the current shop.
        /// </summary>
        /// <param name="player">Player entity</param>
        /// <param name="e"></param>
        private void CloseShop(IPlayerEntity player, NpcShopEventArgs e)
        {
            player.PlayerData.CurrentShopName = null;
        }

        /// <summary>
        /// Buys an item from the current shop.
        /// </summary>
        /// <param name="player">Player entity</param>
        /// <param name="e"></param>
        private void BuyItem(IPlayerEntity player, NpcShopEventArgs e)
        {
            if (!(e is NpcShopBuyItemEventArgs npcEvent))
                throw new ArgumentException("ShopSystem: Invalid event arguments for BuyItem action.");

            if (!WorldServer.Npcs.TryGetValue(player.PlayerData.CurrentShopName, out NpcData npcData))
            {
                Logger.Error($"ShopSystem: Cannot find NPC: {player.PlayerData.CurrentShopName}");
                return;
            }

            if (!npcData.HasShop)
            {
                Logger.Error($"ShopSystem: NPC {npcData.Name} doesn't have a shop.");
                return;
            }

            var currentTab = npcData.Shop.Items[npcEvent.Tab];

            if (npcEvent.Slot < 0 || npcEvent.Slot > currentTab.Count - 1)
            {
                Logger.Error($"ShopSystem: Item slot index was out of tab bounds. Slot: {npcEvent.Slot}");
                return;
            }

            var shopItem = currentTab[npcEvent.Slot];

            if (shopItem.Id != npcEvent.ItemId)
            {
                Logger.Error($"ShopSystem: Shop item id doens't match the item id that {player.Object.Name} is trying to buy.");
                return;
            }

            if (player.PlayerData.Gold < npcEvent.ItemData.Cost)
            {
                Logger.Info($"ShopSystem: {player.Object.Name} doens't have enough gold to buy item {npcEvent.ItemData.Name} at {npcEvent.ItemData.Cost}.");
                WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LACKMONEY);
                return;
            }

            int quantity = npcEvent.Quantity;
            int cost = npcEvent.ItemData.Cost;

            if (npcEvent.ItemData.IsStackable)
            {
                for (var i = 0; i < InventorySystem.EquipOffset; i++)
                {
                    Item inventoryItem = player.Inventory.Items[i];

                    if (inventoryItem.Id == npcEvent.ItemId)
                    {
                        if (inventoryItem.Quantity + quantity > npcEvent.ItemData.PackMax)
                        {
                            int boughtQuantity = inventoryItem.Data.PackMax - inventoryItem.Quantity;

                            quantity -= boughtQuantity;
                            inventoryItem.Quantity = inventoryItem.Data.PackMax;
                            player.PlayerData.Gold -= cost * boughtQuantity;
                        }
                        else
                        {
                            inventoryItem.Quantity += quantity;
                            player.PlayerData.Gold -= cost * quantity;
                            quantity = 0;
                        }

                        WorldPacketFactory.SendUpdateAttributes(player, DefineAttributes.GOLD, player.PlayerData.Gold);
                        WorldPacketFactory.SendItemUpdate(player, UpdateItemType.UI_NUM, inventoryItem.UniqueId, inventoryItem.Quantity);
                    }
                }

                if (quantity > 0)
                {
                    if (!player.Inventory.HasAvailableSlots())
                    {
                        WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LACKSPACE);
                    }
                    else
                    {
                        var item = new Item(npcEvent.ItemId, quantity, -1);

                        if (player.Inventory.CreateItem(item))
                        {
                            player.PlayerData.Gold -= cost;
                            WorldPacketFactory.SendItemCreation(player, item);
                            WorldPacketFactory.SendUpdateAttributes(player, DefineAttributes.GOLD, player.PlayerData.Gold);
                        }
                        else
                        {
                            Logger.Error("ShopSystem: Failed to create item.");
                        }
                    }
                }
            }
            else
            {
                while (quantity > 0)
                {
                    if (!player.Inventory.HasAvailableSlots())
                    {
                        WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LACKSPACE);
                        break;   
                    }

                    var item = new Item(npcEvent.ItemId, 1, -1);

                    if (player.Inventory.CreateItem(item))
                    {
                        player.PlayerData.Gold -= cost;
                        WorldPacketFactory.SendItemCreation(player, item);
                        WorldPacketFactory.SendUpdateAttributes(player, DefineAttributes.GOLD, player.PlayerData.Gold);
                    }
                    else
                    {
                        Logger.Error("ShopSystem: Failed to create item.");
                    }

                    quantity--;   
                }
            }
        }

        /// <summary>
        /// Sells an item to a NPC shop.
        /// </summary>
        /// <param name="player">Player entity</param>
        /// <param name="e"></param>
        private void SellItem(IPlayerEntity player, NpcShopEventArgs e)
        {
            if (!(e is NpcShopSellItemEventArgs npcEvent))
                throw new ArgumentException("ShopSystem: Invalid event arguments for SellItem action.");

            Item itemToSell = player.Inventory.GetItem(npcEvent.ItemUniqueId);

            if (itemToSell?.Data == null)
                throw new InvalidOperationException($"ShopSystem: Cannot find item with unique id: {npcEvent.ItemUniqueId}");

            if (npcEvent.Quantity > itemToSell.Data.PackMax)
                throw new InvalidOperationException($"ShopSystem: Cannot sell more items than the pack max value. {npcEvent.Quantity} > {itemToSell.Data.PackMax}");

            // TODO: make more checks:
            // is a quest item
            // is sealed to character
            // ect...

            int sellPrice = itemToSell.Data.Cost / 4;

            Logger.Debug("Selling item: '{0}' for {1}", itemToSell.Data.Name, sellPrice);

            player.PlayerData.Gold += sellPrice * npcEvent.Quantity;
            itemToSell.Quantity -= npcEvent.Quantity;

            WorldPacketFactory.SendItemUpdate(player, UpdateItemType.UI_NUM, itemToSell.UniqueId, itemToSell.Quantity);
            WorldPacketFactory.SendUpdateAttributes(player, DefineAttributes.GOLD, player.PlayerData.Gold);

            if (itemToSell.Quantity <= 0)
                itemToSell.Reset();
        }
    }
}
