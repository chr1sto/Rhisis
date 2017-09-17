using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Rhisis.Database.Structures;

namespace Rhisis.Database
{
    public abstract class DatabaseContext : DbContext
    {
        /// <summary>
        /// Gets the <see cref="DatabaseContext"/> configuration.
        /// </summary>
        protected DatabaseConfiguration Configuration { get; private set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Character> Characters { get; set; }

        /// <summary>
        /// Creates a new <see cref="DatabaseContext"/> instance.
        /// </summary>
        /// <param name="configuration"></param>
        protected DatabaseContext(DatabaseConfiguration configuration)
        {
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
            catch { }
            finally
            {
                this.Database.Migrate();
            }
        }
    }
}
