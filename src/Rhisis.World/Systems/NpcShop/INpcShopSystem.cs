using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems.NpcShop
{
    public interface INpcShopSystem
    {
        /// <summary>
        /// Opens a NPC shop.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npcObjectId">NPC object id.</param>
        void OpenShop(IPlayerEntity player, uint npcObjectId);

        /// <summary>
        /// Closes a NPC shop.
        /// </summary>
        /// <param name="player">Current player.</param>
        void CloseShop(IPlayerEntity player);

        /// <summary>
        /// Buy a item from a NPC shop.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="shopItemInfo">Bought item information.</param>
        /// <param name="quantity">Bought quantity.</param>
        void BuyItem(IPlayerEntity player, NpcShopItemInfo shopItemInfo, int quantity);

        /// <summary>
        /// Sell an item to a NPC shop.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="playerItemUniqueId">Item unique id.</param>
        /// <param name="quantity">Quantity to sell.</param>
        void SellItem(IPlayerEntity player, int playerItemUniqueId, int quantity);
    }
}
