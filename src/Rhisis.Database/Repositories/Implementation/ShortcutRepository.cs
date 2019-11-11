using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Entities;
using System.Linq;

namespace Rhisis.Database.Repositories.Implementation
{
    /// <summary>
    /// Shortcut repository.
    /// </summary>
    internal sealed class ShortcutRepository : RepositoryBase<DbShortcut>, IShortcutRepository
    {
        /// <summary>
        /// Creates an initialize an <see cref="ShortcutRepository"/>.
        /// </summary>
        /// <param name="context"></param>
        public ShortcutRepository(DbContext context)
            : base(context)
        {
        }

        /// <inheritdoc />
        protected override IQueryable<DbShortcut> GetQueryable()
        {
            return base.GetQueryable()
                .Include(x => x.Character);
        }
    }
}
