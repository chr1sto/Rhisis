using Rhisis.World.Game;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using System.Collections.Generic;

namespace Rhisis.World.Systems.Skills
{
    public interface ISkillSystem : IGameSystemLifeCycle
    {
        void UpdateSkills(IPlayerEntity player, IReadOnlyDictionary<int, int> skillsToUpdate);

        void UseSkill(IPlayerEntity player, SkillInfo skill, int targetObjectId);

        bool CanUseSkill(IPlayerEntity player, SkillInfo skill);
    }
}
