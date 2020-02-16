using Rhisis.Core.Data;
using Rhisis.World.Game;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using System.Collections.Generic;

namespace Rhisis.World.Systems.Skills
{
    public interface ISkillSystem : IGameSystemLifeCycle
    {
        void UpdateSkills(IPlayerEntity player, IReadOnlyDictionary<int, int> skillsToUpdate);

        void UseSkill(IPlayerEntity player, SkillInfo skill, uint targetObjectId, SkillUseType skillUseType);

        bool CanUseSkill(IPlayerEntity player, SkillInfo skill);
    }
}
