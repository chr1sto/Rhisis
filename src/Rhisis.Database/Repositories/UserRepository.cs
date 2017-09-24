using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Structures;

namespace Rhisis.Database.Repositories
{
    public sealed class UserRepository : ARepository<User>
    {
        public UserRepository(DbContext context)
            : base(context)
        {
        }

        protected override IQueryable<User> GetQueryable()
        {
            return base.GetQueryable()
                .Include(x => x.Characters)
                    .ThenInclude(x => x.Items);
        }
    }
}
