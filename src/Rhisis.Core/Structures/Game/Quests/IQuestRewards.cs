using System.Collections.Generic;

namespace Rhisis.Core.Structures.Game.Quests
{
    public interface IQuestRewards
    {
        /// <summary>
        /// Gets the quest gold reward.
        /// </summary>
        int Gold { get; }

        /// <summary>
        /// Gets the quest items reward.
        /// </summary>
        IEnumerable<QuestItem> Items { get; }
    }
}
