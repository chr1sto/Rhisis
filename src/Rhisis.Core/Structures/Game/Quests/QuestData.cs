using Rhisis.Core.Data;
using System.Collections.Generic;

namespace Rhisis.Core.Structures.Game.Quests
{
    public class QuestData
    {
        /// <summary>
        /// Gets or sets the quest name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the quest title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the quest start character.
        /// </summary>
        public string StartCharacter { get; set; }

        /// <summary>
        /// Gets or sets the quest end character.
        /// </summary>
        public string EndCharacter { get; set; }

        /// <summary>
        /// Gets or sets the quest min level to start.
        /// </summary>
        public int MinLevel { get; set; }

        /// <summary>
        /// Gets or sets the quest max level to start.
        /// </summary>
        public int MaxLevel { get; set; }

        /// <summary>
        /// Gets or sets the jobs that can start the quest.
        /// </summary>
        public string[] StartJobs { get; set; }

        /// <summary>
        /// Gets or sets the previous quest id.
        /// </summary>
        public string PreviousQuestId { get; set; }

        /// <summary>
        /// Gets or sets the quest begin dialogs.
        /// </summary>
        public string[] BeginDialogs { get; set; }

        /// <summary>
        /// Gets or sets the quest dialogs when the quest has been accepted.
        /// </summary>
        public string[] AcceptDialogs { get; set; }

        /// <summary>
        /// Gets or sets the quest dialogs when the quest has been declined.
        /// </summary>
        public string[] DeclinedDialogs { get; set; }

        /// <summary>
        /// Gets or sets the quest dialogs when the quest is not finished.
        /// </summary>
        public string[] FailureDialogs { get; set; }

        /// <summary>
        /// Gets or sets the quest dialogs when the quest is completed.
        /// </summary>
        public string[] CompletedDialogs { get; set; }

        /// <summary>
        /// Gets or sets the min gold reward when the quest is completed.
        /// </summary>
        public int MinGold { get; set; }

        /// <summary>
        /// Gets or sets the max gold reward when the quest is completed.
        /// </summary>
        public int MaxGold { get; set; }

        /// <summary>
        /// Gets or sets the min experience reward when the quest is completed.
        /// </summary>
        public int MinExp { get; set; }

        /// <summary>
        /// Gets or sets the max experience reward when the quest is completed.
        /// </summary>
        public int MaxExp { get; set; }

        /// <summary>
        /// Gets or sets the reward items when the quest is completed.
        /// </summary>
        public IEnumerable<QuestItem> RewardItems { get; set; }

        /// <summary>
        /// Gets or sets the job to change once the quest is completed.
        /// </summary>
        public DefineJob.Job? RewardJob { get; set; }

        /// <summary>
        /// Gets or sets the required items to finish the quest.
        /// </summary>
        public IList<QuestItem> EndQuestItems { get; set; }

        /// <summary>
        /// Gets or sets the required monsters to kill to finish the quest.
        /// </summary>
        public IEnumerable<QuestMonster> EndQuestMonsters { get; set; }

        /// <summary>
        /// Gets or sets the required patrols areas to finish the quest.
        /// </summary>
        public IEnumerable<QuestPatrol> EndQuestPatrols { get; set; }
    }
}
