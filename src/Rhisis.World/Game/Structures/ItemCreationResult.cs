using System.Collections;
using System.Collections.Generic;

namespace Rhisis.World.Game.Structures
{
    public struct ItemCreationResult
    {
        public ItemCreationActionType ActionType { get; }

        public Item Item { get; }

        public ItemCreationResult(ItemCreationActionType actionType, Item item)
        {
            ActionType = actionType;
            Item = item;
        }
    }

    public enum ItemCreationActionType
    {
        Add,
        Update,
        Delete
    }
}