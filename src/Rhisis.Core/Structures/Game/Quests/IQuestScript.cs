using System.Collections.Generic;

namespace Rhisis.Core.Structures.Game.Quests
{
    public interface IQuestScript
    {
        /// <summary>
        /// Gets the quest unique id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the quest name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the quest title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets the quest start character.
        /// </summary>
        string StartCharacter { get; }

        /// <summary>
        /// Gets the quest end character.
        /// </summary>
        string EndCharacter { get; }

        /// <summary>
        /// Gets the quest rewards.
        /// </summary>
        IQuestRewards Rewards { get; }

        /// <summary>
        /// Gets the quest start requirements.
        /// </summary>
        IQuestStartRequirements StartRequirements { get; }

        /// <summary>
        /// Gets the quest end conditions.
        /// </summary>
        IQuestEndConditions EndConditions { get; }

        /// <summary>
        /// Gets the quest introduction/begin dialogs.
        /// </summary>
        IEnumerable<string> BeginDialogs { get; }

        /// <summary>
        /// Gets the dialogs when the quest has been accepted by the player.
        /// </summary>
        IEnumerable<string> AcceptedDialogs { get; }

        /// <summary>
        /// Gets the dialogs when the quest has been declined by the player.
        /// </summary>
        IEnumerable<string> DeclinedDialogs { get; }

        /// <summary>
        /// Gets the dialogs when the quest is completed.
        /// </summary>
        IEnumerable<string> CompletedDialogs { get; }

        /// <summary>
        /// Gets the dialogs when the quest is not finished.
        /// </summary>
        IEnumerable<string> NotFinishedDialogs { get; }
    }
}
