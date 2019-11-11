using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Entities;
using System.Linq;

namespace Rhisis.Database.Repositories.Implementation
{
    /// <summary>
    /// Item repository.
    /// </summary>
    internal sealed class ItemRepository : RepositoryBase<DbItem>, IItemRepository
    {
        /// <summary>
        /// Creates an initialize an <see cref="ItemRepository"/>.
        /// </summary>
        /// <param name="context"></param>
        public ItemRepository(DbContext context) 
            : base(context)
        {
        }

        /// <inheritdoc />
        protected override IQueryable<DbItem> GetQueryable()
        {
            return base.GetQueryable()
                .Include(x => x.Character);
        }
    }
}
