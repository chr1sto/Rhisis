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

        public DatabaseContext DatabaseContext { get; }

        /// <summary>
        /// Creates a new <see cref="Database"/> object instance.
        /// </summary>
        public Database(DatabaseContext databaseContext)
        {
            this.DatabaseContext = databaseContext;
            this.InitializeRepositories();
        }

        /// <inheritdoc />
        public void Complete() => this.DatabaseContext.SaveChanges();

        /// <inheritdoc />
        public async Task CompleteAsync() => await this.DatabaseContext.SaveChangesAsync();

        /// <inheritdoc />
        public void Dispose() => this.DatabaseContext.Dispose();

        /// <summary>
        /// Initializes the repositories.
        /// </summary>
        private void InitializeRepositories()
        {
            this.Users = new UserRepository(this.DatabaseContext);
            this.Characters = new CharacterRepository(this.DatabaseContext);
            this.Items = new ItemRepository(this.DatabaseContext);
            this.Mails = new MailRepository(this.DatabaseContext);
            this.TaskbarShortcuts = new ShortcutRepository(this.DatabaseContext);
            this.Quests = new QuestRepository(this.DatabaseContext);
        }
    }
}
