using System;
using System.Linq.Expressions;
using Rhisis.Core.Data;
using Rhisis.Core.IO;
using Rhisis.World.Core.Systems;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Core.Interfaces;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Inventory;

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
        /// <param name="player"></param>
        /// <param name="e"></param>
        private void OpenShop(IPlayerEntity player, NpcShopEventArgs e)
        {
            if (!(e is NpcShopOpenEventArgs npcEvent))
                throw new ArgumentException("ShopSystem: Invalid event arguments for OpenShop action.");

            var npc = player.Context.FindEntity<INpcEntity>(npcEvent.NpcObjectId);

            if (npc == null)
            {
                Logger.Error("Cannot find NPC with object id : {0}", npcEvent.NpcObjectId);
                return;
            }

            if (npc.Shop == null)
            {
                Logger.Error("NPC '{0}' doesn't have a shop.", npc.ObjectComponent.Name);
                return;
            }

            WorldPacketFactory.SendNpcShop(player, npc);
        }

        private void BuyItem(IPlayerEntity player, NpcShopEventArgs e)
        {
            if (!(e is NpcShopBuyItemEventArgs npcEvent))
                throw new ArgumentException("ShopSystem: Invalid event arguments for BuyItem action.");

            if (player.PlayerComponent.Gold < npcEvent.ItemData.Cost)
            {
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
                            player.PlayerComponent.Gold -= cost * boughtQuantity;
                        }
                        else
                        {
                            inventoryItem.Quantity += quantity;
                            player.PlayerComponent.Gold -= cost * quantity;
                            quantity = 0;
                        }

                        WorldPacketFactory.SendUpdateAttributes(player, DefineAttributes.GOLD, player.PlayerComponent.Gold);
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
                            player.PlayerComponent.Gold -= cost;
                            WorldPacketFactory.SendItemCreation(player, item);
                            WorldPacketFactory.SendUpdateAttributes(player, DefineAttributes.GOLD, player.PlayerComponent.Gold);
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
                        player.PlayerComponent.Gold -= cost;
                        WorldPacketFactory.SendItemCreation(player, item);
                        WorldPacketFactory.SendUpdateAttributes(player, DefineAttributes.GOLD, player.PlayerComponent.Gold);
                    }
                    else
                    {
                        Logger.Error("ShopSystem: Failed to create item.");
                    }

                    quantity--;   
                }
            }
        }

        private void SellItem(IPlayerEntity player, NpcShopEventArgs e)
        {
            
        }
    }
}
