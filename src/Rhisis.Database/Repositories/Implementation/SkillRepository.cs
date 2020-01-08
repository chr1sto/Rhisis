using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Entities;

namespace Rhisis.Database.Repositories.Implementation
{
    public class SkillRepository : RepositoryBase<DbSkill>, ISkillRepository
    {
        /// <summary>
        /// Creates a new <see cref="SkillRepository"/> instance.
        /// </summary>
        /// <param name="context">Database context.</param>
        public SkillRepository(DbContext context)
            : base(context)
        {
        }
    }
}
