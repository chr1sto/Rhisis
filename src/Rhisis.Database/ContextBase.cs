using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Rhisis.Database.Interfaces;
using Rhisis.Database.Repositories;
using Rhisis.Database.Structures;

namespace Rhisis.Database
{
    public abstract class DatabaseContext : DbContext
    {
        private readonly IRepository<User> _users;
        private readonly IRepository<Character> _characters;
        private readonly IRepository<Item> _items;
        
        /// <summary>
        /// Gets the <see cref="User"/> repository.
        /// </summary>
        public IRepository<User> Users => this._users;

        /// <summary>
        /// Gets the <see cref="Character"/> repository.
        /// </summary>
        public IRepository<Character> Characters => this._characters;

        /// <summary>
        /// Gets the <see cref="Item"/> repository.
        /// </summary>
        public IRepository<Item> Items => this._items;

        /// <summary>
        /// Gets the <see cref="DatabaseContext"/> configuration.
        /// </summary>
        protected DatabaseConfiguration Configuration { get; private set; }

        /// <summary>
        /// Gets or sets the database users.
        /// </summary>
        internal DbSet<User> _Users { get; set; }

        /// <summary>
        /// Gets or sets the database characters.
        /// </summary>
        internal DbSet<Character> _Characters { get; set; }

        /// <summary>
        /// Gets or sets the database items.
        /// </summary>
        internal DbSet<Item> _Items { get; set; }

        /// <summary>
        /// Creates a new <see cref="DatabaseContext"/> instance.
        /// </summary>
        /// <param name="configuration"></param>
        protected DatabaseContext(DatabaseConfiguration configuration)
        {
            this._users = new UserRepository(this);
            this._characters = new CharacterRepository(this);
            this._items = new ItemRepository(this);

            this.Configuration = configuration;
        }

        /// <summary>
        /// Called when the configuration of the DbContext has begun.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Creates the database.
        /// </summary>
        /// <returns></returns>
        public virtual bool CreateDatabase()
        {
            return this.Database.EnsureCreated();
        }

        /// <summary>
        /// Check if the database exists.
        /// </summary>
        /// <returns></returns>
        public virtual bool DatabaseExists()
        {
            return (this.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();
        }

        /// <summary>
        /// Processes the database migration.
        /// </summary>
        public virtual void Migrate()
        {
            try
            {
                var databaseCreator = this.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                databaseCreator.CreateTables();
            }
            catch
            {
                // Nothing to do.
            }
            finally
            {
                this.Database.Migrate();
            }
        }
    }
}
