using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Structures;

namespace Rhisis.Database.Repositories
{
    /// <summary>
    /// Character repository.
    /// </summary>
    public sealed class CharacterRepository : ARepository<Character>
    {
        /// <summary>
        /// Creates and initialize the <see cref="CharacterRepository"/>.
        /// </summary>
        /// <param name="context"></param>
        public CharacterRepository(DbContext context) 
            : base(context)
        {
        }

        /// <summary>
        /// Include other objects for each requests.
        /// </summary>
        /// <returns></returns>
        protected override IQueryable<Character> GetQueryable()
        {
            return base.GetQueryable()
                .Include(x => x.User)
                .Include(x => x.Items);
        }
    }
}
