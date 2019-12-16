using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Core.Structures.Game.Quests
{
    public class QuestData
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string StartCharacter { get; set; }

        public int MinLevel { get; set; }

        public int MaxLevel { get; set; }

        public string[] StartJobs { get; set; }

        public string[] BeginDialogs { get; set; }

        public string[] AcceptDialogs { get; set; }

        public string[] DeclinedDialogs { get; set; }

        public string[] FailureDialogs { get; set; }

        public string[] CompletedDialogs { get; set; }

        public int MinGold { get; set; }

        public int MaxGold { get; set; }
    }
}
