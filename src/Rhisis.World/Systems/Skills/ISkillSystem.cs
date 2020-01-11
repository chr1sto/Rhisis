using Rhisis.World.Game;
using Rhisis.World.Game.Entities;
using System.Collections.Generic;

namespace Rhisis.World.Systems.Skills
{
    public interface ISkillSystem : IGameSystemLifeCycle
    {
        void UpdateSkills(IPlayerEntity player, IReadOnlyDictionary<int, int> skillsToUpdate);
    }
}
