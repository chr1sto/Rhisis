using Rhisis.Database.Entities;
using System.Collections.Generic;

namespace Rhisis.Database.Repositories
{
    public interface ICharacterRepository : IRepository<DbCharacter>
    {
        IEnumerable<DbCharacter> GetCharacters(int userId, bool includeDeletedCharacters = false);

        DbCharacter GetCharacter(int characterId);
    }
}
