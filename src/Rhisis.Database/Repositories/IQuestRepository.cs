using Rhisis.Database.Entities;
using System.Collections.Generic;

namespace Rhisis.Database.Repositories
{
    public interface IQuestRepository : IRepository<DbQuest>
    {
        /// <summary>
        /// Gets the quests of a character.
        /// </summary>
        /// <param name="characterId">Character id.</param>
        /// <returns>Collection of <see cref="DbQuest"/> entities.</returns>
        IEnumerable<DbQuest> GetCharactersQuests(int characterId);
    }
}
