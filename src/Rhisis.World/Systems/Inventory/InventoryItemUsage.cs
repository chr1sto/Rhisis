using Microsoft.Extensions.Logging;
using Rhisis.Core.Common.Formulas;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Helpers;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using System;
using System.Collections.Generic;

namespace Rhisis.World.Systems.Inventory
{
    internal sealed class InventoryItemUsage
    {
        private readonly ILogger<InventoryItemUsage> _logger;

        /// <summary>
        /// Creates a new <see cref="InventoryItemUsage"/> instance.
        /// </summary>
        public InventoryItemUsage()
        {
            this._logger = DependencyContainer.Instance.Resolve<ILogger<InventoryItemUsage>>();
        }

        /// <summary>
        /// Check if the given item is equipable by a player.
        /// </summary>
        /// <param name="player">Player trying to equip an item.</param>
        /// <param name="item">Item to equip.</param>
        /// <returns>True if the player can equip the item; false otherwise.</returns>
        public bool IsItemEquipable(IPlayerEntity player, Item item)
        {
            if (item.Data.ItemSex != player.VisualAppearance.Gender)
            {
                this._logger.LogDebug("Wrong sex for armor");
                WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_WRONGSEX, item.Data.Name);
                return false;
            }

            if (player.Object.Level < item.Data.LimitLevel)
            {
                this._logger.LogDebug("Player level to low");
                WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_REQLEVEL, item.Data.LimitLevel.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// Uses an item from the player's inventory.
        /// </summary>
        /// <param name="player">Player using an item.</param>
        /// <param name="itemToUse">Item to use.</param>
        public void UseItem(IPlayerEntity player, Item itemToUse)
        {
            this._logger.LogTrace($"{player.Object.Name} want to use {itemToUse.Data.Name}.");

            // TODO: check if item is ready to use.

            switch (itemToUse.Data.ItemKind2)
            {
                case ItemKind2.FOOD:
                    this.UseFoodItem(player, itemToUse);
                    break;
            }
        }

        /// <summary>
        /// Use food item.
        /// </summary>
        /// <param name="player">Player using the item.</param>
        /// <param name="foodItemToUse">Food item to use.</param>
        public void UseFoodItem(IPlayerEntity player, Item foodItemToUse)
        {
            foreach (KeyValuePair<DefineAttributes, int> parameter in foodItemToUse.Data.Params)
            {
                if (parameter.Key == DefineAttributes.HP || parameter.Key == DefineAttributes.MP || parameter.Key == DefineAttributes.FP)
                {
                    int currentPoints = PlayerHelper.GetPoints(player, parameter.Key);
                    int maxPoints = PlayerHelper.GetMaxPoints(player, parameter.Key);
                    int itemMaxRecovery = foodItemToUse.Data.AbilityMin;

                    if (parameter.Value >= 0)
                    {
                        if (currentPoints >= maxPoints)
                        {
                            float limitedRecovery = parameter.Value * 0.3f;

                            if (currentPoints + limitedRecovery > maxPoints)
                            {
                                currentPoints = maxPoints;
                            }
                            else
                            {
                                switch (parameter.Key)
                                {
                                    case DefineAttributes.HP: WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LIMITHP); break;
                                    case DefineAttributes.MP: WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LIMITMP); break;
                                    case DefineAttributes.FP: WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LIMITFP); break;
                                }

                                currentPoints += (int)limitedRecovery;
                            }
                        }
                        else
                        {
                            currentPoints = Math.Min(currentPoints + parameter.Value, maxPoints);
                        }
                    }

                    PlayerHelper.SetPoints(player, parameter.Key, currentPoints);
                    WorldPacketFactory.SendUpdateAttributes(player, parameter.Key, PlayerHelper.GetPoints(player, parameter.Key));
                }
                else
                {
                    // TODO: food triggers a skill.
                }
            }
        }
    }
}
