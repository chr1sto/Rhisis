using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Entities;

namespace Rhisis.Database.Repositories
{
    /// <summary>
    /// User repository.
    /// </summary>
    public sealed class UserRepository : RepositoryBase<User>
    {
        /// <summary>
        /// Creates and initialize an <see cref="UserRepository"/>.
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(DbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Include other objects for each requests.
        /// </summary>
        /// <returns></returns>
        protected override IQueryable<User> GetQueryable()
        {
            return base.GetQueryable()
                .Include(x => x.Characters)
                    .ThenInclude(x => x.Items);
        }
    }
}
