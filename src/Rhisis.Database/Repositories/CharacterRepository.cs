using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database.Entities;
using Rhisis.Database.Interfaces;

namespace Rhisis.Database.Repositories
{
    /// <summary>
    /// Character repository.
    /// </summary>
    [Injectable]
    public sealed class CharacterRepository : RepositoryBase<DbCharacter>, ICharacterRepository
    {
        /// <summary>
        /// Creates a new <see cref="CharacterRepository"/> instance.
        /// </summary>
        public CharacterRepository()
        {
        }

        /// <summary>
        /// Creates and initialize the <see cref="CharacterRepository"/>.
        /// </summary>
        /// <param name="context"></param>
        public CharacterRepository(DbContext context) 
            : base(context)
        {
        }

        /// <inheritdoc />
        protected override IQueryable<DbCharacter> GetQueryable(DbContext context)
        {
            return base.GetQueryable(context)
                .Include(x => x.User)
                .Include(x => x.Items);
        }
    }
}
