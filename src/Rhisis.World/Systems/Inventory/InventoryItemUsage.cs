using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;

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
        }
    }
}
