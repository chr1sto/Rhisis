using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database.Entities;
using Rhisis.Database.Interfaces;

namespace Rhisis.Database.Repositories
{
    /// <summary>
    /// User repository.
    /// </summary>
    [Injectable]
    public sealed class UserRepository : RepositoryBase<DbUser>, IUserRepository
    {
        /// <summary>
        /// Creates a new <see cref="UserRepository"/> instance.
        /// </summary>
        public UserRepository()
        {
        }

        /// <summary>
        /// Creates and initialize an <see cref="UserRepository"/>.
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(DbContext context)
            : base(context)
        {
        }

        /// <inheritdoc />
        protected override IQueryable<DbUser> GetQueryable(DbContext context)
        {
            return base.GetQueryable(context)
                .Include(x => x.Characters)
                    .ThenInclude(x => x.Items);
        }
    }
}
