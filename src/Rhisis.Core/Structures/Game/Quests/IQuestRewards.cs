using Rhisis.Core.Data;
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
        /// Gets the quest experience reward.
        /// </summary>
        long Experience { get; }

        /// <summary>
        /// Gets the quest items reward.
        /// </summary>
        IEnumerable<QuestItem> Items { get; }

        /// <summary>
        /// Gets a boolean value that indiciates if the system should restat the player.
        /// </summary>
        bool Restat { get; }

        /// <summary>
        /// Gets a boolean value that indiciates if the systme should reskill the player.
        /// </summary>
        bool Reskill { get; }

        /// <summary>
        /// Gets the number of skills points to offer to the player.
        /// </summary>
        ushort SkillPoints { get; }

        /// <summary>
        /// Gets the reward job based on the player information.
        /// </summary>
        /// <param name="player">Current player entity information.</param>
        /// <returns></returns>
        DefineJob.Job GetJob(object player);

        /// <summary>
        /// Checks if the quest has a job reward.
        /// </summary>
        /// <returns>True if the quest has job reward; false otherwise.</returns>
        bool HasJobReward();
    }
}
