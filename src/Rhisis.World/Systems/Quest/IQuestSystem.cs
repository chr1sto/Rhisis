using Rhisis.Core.Structures.Game.Dialogs;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems.Quest
{
    public interface IQuestSystem
    {
        /// <summary>
        /// Initialize the player's quest diairy.
        /// </summary>
        /// <param name="player">Current player.</param>
        void Initialize(IPlayerEntity player);

        /// <summary>
        /// Saves the player's quest diary.
        /// </summary>
        /// <param name="player">Current player.</param>
        void Save(IPlayerEntity player);

        /// <summary>
        /// Check if the player can start the given quest script.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="quest">Quest script to start.</param>
        /// <returns>True if the player can start the quest; false otherwise.</returns>
        bool CanStartQuest(IPlayerEntity player, IQuestScript quest);

        /// <summary>
        /// Process a quest script state.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Current NPC holding the quest.</param>
        /// <param name="quest">Quest script.</param>
        /// <param name="state">Quest script state.</param>
        void ProcessQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest, QuestStateType state);

        /// <summary>
        /// Creates a new quest <see cref="DialogLink"/>.
        /// </summary>
        /// <param name="quest">Quest.</param>
        /// <returns>Quest <see cref="DialogLink"/>.</returns>
        DialogLink CreateQuestLink(IQuestScript quest);

        /// <summary>
        /// Check or uncheck a quest.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="questId">Quest id to check.</param>
        /// <param name="checkedState">Check state.</param>
        void CheckQuest(IPlayerEntity player, int questId, bool checkedState);
    }
}
