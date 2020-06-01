using System.Collections.Generic;

namespace Rhisis.Core.Structures.Game.Quests
{
    public interface IQuestEndConditions
    {
        /// <summary>
        /// Gets the items description to complete the quest.
        /// </summary>
        IEnumerable<QuestItem> Items { get; }

        /// <summary>
        /// Gets the monsters information to kill to complete the quest.
        /// </summary>
        IEnumerable<QuestMonster> Monsters { get; }

        /// <summary>
        /// Gets the patrols to be done to complete the quest.
        /// </summary>
        IEnumerable<QuestPatrol> Patrols { get; }
    }
}
