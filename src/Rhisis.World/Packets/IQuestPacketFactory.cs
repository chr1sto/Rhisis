using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using System.Collections.Generic;

namespace Rhisis.World.Packets
{
    public interface IQuestPacketFactory
    {
        /// <summary>
        /// Send the quest information to the given player.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="quest">Quest information.</param>
        void SendQuest(IPlayerEntity player, QuestInfo quest);

        /// <summary>
        /// Sends the checked quests to the given player.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="checkedQuests">Checked quests.</param>
        void SendCheckedQuests(IPlayerEntity player, IEnumerable<QuestInfo> checkedQuests);
    }
}
