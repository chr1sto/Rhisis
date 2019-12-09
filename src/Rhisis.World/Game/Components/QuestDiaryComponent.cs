using Rhisis.World.Game.Structures;
using Sylver.Network.Data;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Components
{
    public class QuestDiaryComponent
    {
        public IList<Quest> ActiveQuests { get; }

        public IList<Quest> FinishedQuests { get; }

        public QuestDiaryComponent()
        {
            this.ActiveQuests = new List<Quest>();
            this.FinishedQuests = new List<Quest>();
        }

        public QuestDiaryComponent(IEnumerable<Quest> quests)
        {
            this.ActiveQuests = quests.Where(x => !x.IsFinished).ToList();
            this.FinishedQuests = quests.Where(x => x.IsFinished).ToList();
        }

        public bool HasQuest(int questId) 
            => this.ActiveQuests.Any(x => x.QuestId == questId) || this.FinishedQuests.Any(x => x.QuestId == questId);

        public IEnumerable<Quest> GetCheckedQuests() => this.ActiveQuests.Where(x => x.IsChecked);

        public void Serialize(INetPacketStream packet)
        {
            packet.Write((byte)this.ActiveQuests.Count());
            foreach (Quest quest in this.ActiveQuests)
            {
                quest.Serialize(packet);
            }

            packet.Write((byte)this.FinishedQuests.Count());
            foreach (Quest quest in this.FinishedQuests)
            {
                packet.Write((short)quest.QuestId);
            }

            IEnumerable<Quest> checkedQuests = this.GetCheckedQuests();

            packet.Write((byte)checkedQuests.Count());
            foreach (Quest quest in checkedQuests)
            {
                packet.Write((short)quest.QuestId);
            }
        }
    }
}
