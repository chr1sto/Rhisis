namespace Rhisis.Core.Structures.Game.Quests
{
    public class QuestItemDrop
    {
        /// <summary>
        /// Gets or sets the monster id where the item can be dropped.
        /// </summary>
        public string MonsterId { get; set; }

        /// <summary>
        /// Gets or sets the item id that can be dropped.
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the drop probability for the specified monster.
        /// </summary>
        public long Probability { get; set; }

        /// <summary>
        /// Gets or sets the item drop quantity.
        /// </summary>
        public int Quantity { get; set; }
    }
}
