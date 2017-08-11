using Microsoft.EntityFrameworkCore;

namespace Rhisis.Database
{
    public abstract class DatabaseContext : DbContext
    {
        protected DatabaseConfiguration Configuration { get; private set; }

        // TODO: add repository pattern and DbSets

        protected DatabaseContext(DatabaseConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public abstract void Migrate();
    }
}
