using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Structures;

namespace Rhisis.Database.Repositories
{
    public sealed class ItemRepository : ARepository<Item>
    {
        public ItemRepository(DbContext context) 
            : base(context)
        {
        }

        protected override IQueryable<Item> GetQueryable()
        {
            return base.GetQueryable()
                .Include(x => x.Character);
        }
    }
}
