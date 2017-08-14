using Microsoft.EntityFrameworkCore;

namespace Rhisis.Database
{
    public abstract class DatabaseContext : DbContext
    {
        /// <summary>
        /// Gets the <see cref="DatabaseContext"/> configuration.
        /// </summary>
        protected DatabaseConfiguration Configuration { get; private set; }
        
        // TODO: add repository pattern and DbSets

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
        public abstract bool CreateDatabase();

        /// <summary>
        /// Check if the database exists.
        /// </summary>
        /// <returns></returns>
        public abstract bool DatabaseExists();

        /// <summary>
        /// Processes the database migration.
        /// </summary>
        public abstract void Migrate();
    }
}
