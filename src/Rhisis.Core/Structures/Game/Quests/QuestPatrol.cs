namespace Rhisis.Core.Structures.Game.Quests
{
    public class QuestPatrol
    {
        /// <summary>
        /// Gets or sets the map id.
        /// </summary>
        public string MapId { get; set; }

        /// <summary>
        /// Gets or sets the left coordinate of the patrol region.
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// Gets or sets the top coordinate of the patrol region.
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Gets or sets the right coordinate of the patrol region.
        /// </summary>
        public int Right { get; set; }

        /// <summary>
        /// Gets or sets the bottom coordinate of the patrol region.
        /// </summary>
        public int Bottom { get; set; }
    }
}
