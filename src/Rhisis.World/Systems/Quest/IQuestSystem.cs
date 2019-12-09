using Rhisis.Core.Structures.Game.Dialogs;
using Rhisis.Core.Structures.Game.Quests;
using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems.Quest
{
    public interface IQuestSystem
    {
        void Initialize(IPlayerEntity player);

        bool CanStartQuest(IPlayerEntity player, IQuestScript quest);

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
