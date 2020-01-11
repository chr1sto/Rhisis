using Rhisis.Database.Entities;
using System.Collections.Generic;

namespace Rhisis.Database.Repositories
{
    public interface ISkillRepository : IRepository<DbSkill>
    {
        /// <summary>
        /// Gets the character skills.
        /// </summary>
        /// <param name="characterId">Character Id.</param>
        /// <returns>Collection of <see cref="DbSkill"/> attached to the given character id.</returns>
        IEnumerable<DbSkill> GetCharacterSkills(int characterId);
    }
}
