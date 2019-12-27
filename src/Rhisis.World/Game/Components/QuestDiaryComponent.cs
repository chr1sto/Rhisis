using Rhisis.World.Game.Structures;
using Sylver.Network.Data;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Components
{
    public class QuestDiaryComponent
    {
        public IList<QuestInfo> ActiveQuests { get; }

        public IList<QuestInfo> FinishedQuests { get; }

        public QuestDiaryComponent()
        {
            this.ActiveQuests = new List<QuestInfo>();
            this.FinishedQuests = new List<QuestInfo>();
        }

        public QuestDiaryComponent(IEnumerable<QuestInfo> quests)
        {
            this.ActiveQuests = quests.Where(x => !x.IsFinished).ToList();
            this.FinishedQuests = quests.Where(x => x.IsFinished).ToList();
        }

        public bool HasQuest(int questId) 
            => this.ActiveQuests.Any(x => x.QuestId == questId) || this.FinishedQuests.Any(x => x.QuestId == questId);

        public IEnumerable<QuestInfo> GetCheckedQuests() => this.ActiveQuests.Where(x => x.IsChecked);

        public void Serialize(INetPacketStream packet)
        {
            packet.Write((byte)this.ActiveQuests.Count());
            foreach (QuestInfo quest in this.ActiveQuests)
            {
                quest.Serialize(packet);
            }

            packet.Write((byte)this.FinishedQuests.Count());
            foreach (QuestInfo quest in this.FinishedQuests)
            {
                packet.Write((short)quest.QuestId);
            }

            IEnumerable<QuestInfo> checkedQuests = this.GetCheckedQuests();

            packet.Write((byte)checkedQuests.Count());
            foreach (QuestInfo quest in checkedQuests)
            {
                packet.Write((short)quest.QuestId);
            }
        }
    }
}
