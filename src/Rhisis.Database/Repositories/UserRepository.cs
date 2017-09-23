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
    }
}
