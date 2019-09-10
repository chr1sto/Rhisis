using Microsoft.EntityFrameworkCore;
using Rhisis.Database.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Database.Repositories.Implementation
{
    public class QuestRepository : RepositoryBase<DbQuest>, IQuestRepository
    {
        /// <summary>
        /// Creates a new <see cref="QuestRepository"/> instance.
        /// </summary>
        /// <param name="context">Database context.</param>
        public QuestRepository(DbContext context) 
            : base(context)
        {
        }

        /// <inheritdoc />
        public IEnumerable<DbQuest> GetCharactersQuests(int characterId)
        {
            return this.GetAll(x => x.CharacterId == characterId);
        }

        /// <inheritdoc />
        protected override IQueryable<DbQuest> GetQueryable()
        {
            return base.GetQueryable()
                .Include(x => x.QuestActions);
        }
    }
}
