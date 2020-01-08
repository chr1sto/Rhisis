using System.Threading.Tasks;
using Rhisis.Database.Context;
using Rhisis.Database.Repositories;
using Rhisis.Database.Repositories.Implementation;

namespace Rhisis.Database
{
    public sealed class Database : IDatabase
    {
        /// <inheritdoc />
        public IUserRepository Users { get; set; }

        /// <inheritdoc />
        public ICharacterRepository Characters { get; set; }

        /// <inheritdoc />
        public IItemRepository Items { get; set; }

        public IMailRepository Mails { get; set; }

        /// <inheritdoc />
        public IShortcutRepository TaskbarShortcuts { get; private set; }

        /// <inheritdoc />
        public IQuestRepository Quests { get; private set; }

        /// <inheritdoc />
        public ISkillRepository Skills { get; private set; }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        public DatabaseContext DatabaseContext { get; }

        /// <summary>
        /// Creates a new <see cref="Database"/> object instance.
        /// </summary>
        public Database(DatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
            InitializeRepositories();
        }

        /// <inheritdoc />
        public void Complete() => DatabaseContext.SaveChanges();

        /// <inheritdoc />
        public async Task CompleteAsync() => await DatabaseContext.SaveChangesAsync();

        /// <inheritdoc />
        public void Dispose() => DatabaseContext.Dispose();

        /// <summary>
        /// Initializes the repositories.
        /// </summary>
        private void InitializeRepositories()
        {
            Users = new UserRepository(DatabaseContext);
            Characters = new CharacterRepository(DatabaseContext);
            Items = new ItemRepository(DatabaseContext);
            Mails = new MailRepository(DatabaseContext);
            TaskbarShortcuts = new ShortcutRepository(DatabaseContext);
            Quests = new QuestRepository(DatabaseContext);
            Skills = new SkillRepository(DatabaseContext);
        }
    }
}
