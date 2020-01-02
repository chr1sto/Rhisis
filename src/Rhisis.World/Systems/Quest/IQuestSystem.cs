using Rhisis.Core.Structures.Game.Quests;
using Rhisis.World.Game;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems.Quest
{
    /// <summary>
    /// Provides a mechansim to manage player quests.
    /// </summary>
    public interface IQuestSystem : IGameSystemLifeCycle
    {
        /// <summary>
        /// Check if the player can start the given quest script.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Current npc.</param>
        /// <param name="quest">Quest script to start.</param>
        /// <returns>True if the player can start the quest; false otherwise.</returns>
        bool CanStartQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest);

        /// <summary>
        /// Check if the player can finish quests from the given npc.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Current npc.</param>
        /// <param name="quest">Quest sript to finish.</param>
        /// <returns>True if the player can finish a quest; false otherwise.</returns>
        bool CanFinishQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest);

        /// <summary>
        /// Process a quest script state.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Current NPC holding the quest.</param>
        /// <param name="quest">Quest script.</param>
        /// <param name="state">Quest script state.</param>
        void ProcessQuest(IPlayerEntity player, INpcEntity npc, IQuestScript quest, QuestStateType state);

        /// <summary>
        /// Check or uncheck a quest.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="questId">Quest id to check.</param>
        /// <param name="checkedState">Check state.</param>
        void CheckQuest(IPlayerEntity player, int questId, bool checkedState);

        /// <summary>
        /// Sends the quest dialogs for available and in progress quests.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="npc">Npc holing quests.</param>
        void SendQuestsInfo(IPlayerEntity player, INpcEntity npc);

        /// <summary>
        /// Updates the player's quest diary.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="actionType">Current quest action.</param>
        /// <param name="values">Update values.</param>
        void UpdateQuestDiary(IPlayerEntity player, QuestActionType actionType, params object[] values);
    }
}
