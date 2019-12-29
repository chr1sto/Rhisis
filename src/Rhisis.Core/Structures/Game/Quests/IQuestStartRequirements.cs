using Rhisis.Core.Data;
using System.Collections.Generic;

namespace Rhisis.Core.Structures.Game.Quests
{
    public interface IQuestStartRequirements
    {
        /// <summary>
        /// Gets the minimum level the player can start the quest.
        /// </summary>
        int MinLevel { get; }

        /// <summary>
        /// Gets the maximum level the player can start the quest.
        /// </summary>
        int MaxLevel { get; }

        /// <summary>
        /// Gets the jobs the player must have to start the quest.
        /// </summary>
        IEnumerable<DefineJob.Job> Jobs { get; }

        /// <summary>
        /// Gets the previous quest id.
        /// </summary>
        string PreviousQuestId { get; }
    }
}
