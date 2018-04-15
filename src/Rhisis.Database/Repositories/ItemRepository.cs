using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Entities;

namespace Rhisis.Database.Repositories
{
    /// <summary>
    /// Item repository.
    /// </summary>
    public sealed class ItemRepository : RepositoryBase<Item>
    {
        /// <summary>
        /// Creates an initialize an <see cref="ItemRepository"/>.
        /// </summary>
        /// <param name="context"></param>
        public ItemRepository(DbContext context) 
            : base(context)
        {
        }

        /// <summary>
        /// Include other objects for each requests.
        /// </summary>
        /// <returns></returns>
        protected override IQueryable<Item> GetQueryable()
        {
            return base.GetQueryable()
                .Include(x => x.Character);
        }
    }
}
