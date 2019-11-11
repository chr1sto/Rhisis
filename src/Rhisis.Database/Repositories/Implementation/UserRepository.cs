using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Entities;
using System.Linq;

namespace Rhisis.Database.Repositories.Implementation
{
    /// <summary>
    /// User repository.
    /// </summary>
    internal sealed class UserRepository : RepositoryBase<DbUser>, IUserRepository
    {
        /// <summary>
        /// Creates and initialize an <see cref="UserRepository"/>.
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(DbContext context)
            : base(context)
        {
        }

        /// <inheritdoc />
        public DbUser GetUser(string username) 
            => this._context.Set<DbUser>().AsQueryable().AsNoTracking().FirstOrDefault(x => x.Username == username);

        /// <inheritdoc />
        public DbUser GetUser(string username, string password) 
            => this._context.Set<DbUser>().AsQueryable().AsNoTracking().FirstOrDefault(x => x.Username == username && x.Password == password);

        /// <inheritdoc />
        protected override IQueryable<DbUser> GetQueryable()
        {
            return base.GetQueryable()
                .Include(x => x.Characters)
                    .ThenInclude(x => x.Items)
                .Include(x => x.Characters)
                    .ThenInclude(x => x.ReceivedMails)
                .Include(x => x.Characters)
                    .ThenInclude(x => x.SentMails)
                .Include(x => x.Characters)
                    .ThenInclude(x => x.TaskbarShortcuts);
        }
    }
}
