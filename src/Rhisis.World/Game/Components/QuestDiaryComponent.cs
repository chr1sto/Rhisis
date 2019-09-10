using Rhisis.World.Game.Structures;
using System.Collections.Generic;

namespace Rhisis.World.Game.Components
{
    public class QuestDiaryComponent
    {
        public IList<Quest> ActiveQuests { get; }

        public IList<Quest> CheckedQuests { get; }

        public IList<Quest> FinishedQuests { get; }

        public QuestDiaryComponent()
        {
            this.ActiveQuests = new List<Quest>();
            this.CheckedQuests = new List<Quest>();
            this.FinishedQuests = new List<Quest>();
        }
    }
}
