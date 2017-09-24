using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Structures;

namespace Rhisis.Database.Repositories
{
    public sealed class CharacterRepository : ARepository<Character>
    {
        public CharacterRepository(DbContext context) 
            : base(context)
        {
        }

        protected override IQueryable<Character> GetQueryable()
        {
            return base.GetQueryable()
                .Include(x => x.User)
                .Include(x => x.Items);
        }
    }
}
