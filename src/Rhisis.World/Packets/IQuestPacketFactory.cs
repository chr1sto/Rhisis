using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using System.Collections.Generic;

namespace Rhisis.World.Packets
{
    public interface IQuestPacketFactory
    {
        void SendQuest(IPlayerEntity player, QuestInfo quest);

        void SendCheckedQuests(IPlayerEntity player, IEnumerable<QuestInfo> checkedQuests);
    }
}
