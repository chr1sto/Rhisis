using Rhisis.Core.Common.Game.Structures;
using Rhisis.World.Game.Core.Systems;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Taskbar.EventArgs
{
    public class TaskbarSkillEventArgs : SystemEventArgs
    {
        public List<Shortcut> Skills { get; }

        public TaskbarSkillEventArgs(List<Shortcut> skills)
        {
            Skills = skills;
        }

        public override bool CheckArguments()
        {
            return Skills != null && !Skills.Any(x => x.SlotIndex >= TaskbarSystem.MaxTaskbarQueue);
        }
    }
}