using Rhisis.Core.Common;

namespace Rhisis.Core.Structures.Game.Quests
{
    public class QuestItem
    {
        /// <summary>
        /// Gets or sets the quest item id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the quest item quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the quest item sex.
        /// </summary>
        public GenderType Sex { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates if this item has to be removed from the inventory
        /// once the quest is completed.
        /// </summary>
        public bool Remove { get; set; }
    }
}
