using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database.Entities;
using Rhisis.Database.Interfaces;

namespace Rhisis.Database.Repositories
{
    /// <summary>
    /// Item repository.
    /// </summary>
    [Repository]
    public sealed class ItemRepository : RepositoryBase<DbItem>, IItemRepository
    {
        /// <summary>
        /// Creates a new <see cref="ItemRepository"/> instance.
        /// </summary>
        public ItemRepository()
        {
        }

        /// <summary>
        /// Creates an initialize an <see cref="ItemRepository"/>.
        /// </summary>
        /// <param name="context"></param>
        public ItemRepository(DbContext context) 
            : base(context)
        {
        }

        /// <inheritdoc />
        protected override IQueryable<DbItem> GetQueryable(DbContext context)
        {
            return base.GetQueryable(context)
                .Include(x => x.Character);
        }
    }
}
