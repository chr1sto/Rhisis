using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Systems.Inventory
{
    public interface IInventoryItemUsage
    {
        /// <summary>
        /// Use food item.
        /// </summary>
        /// <param name="player">Player using the item.</param>
        /// <param name="foodItemToUse">Food item to use.</param>
        void UseFoodItem(IPlayerEntity player, Item foodItemToUse);

        /// <summary>
        /// Uses blinkwing items.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="blinkwing"></param>
        void UseBlinkwingItem(IPlayerEntity player, Item blinkwing);

        /// <summary>
        /// Uses fireworks items.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="magicItem"></param>
        void UseMagicItem(IPlayerEntity player, Item magicItem);

        /// <summary>
        /// Uses perin items.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="perinItem"></param>
        void UsePerin(IPlayerEntity player, Item perinItem);
    }
}
