using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Rhisis.Core.Common;
using Rhisis.Core.Helpers;

namespace Rhisis.Database
{
    public sealed class DatabaseFactory : Singleton<DatabaseFactory>, IDesignTimeDbContextFactory<DatabaseContext>
    {
        private DatabaseConfiguration _databaseConfiguration;

        /// <summary>
        /// Initialize the <see cref="DatabaseFactory"/>.
        /// </summary>
        /// <param name="databaseConfigurationPath"></param>
        public void Initialize(string databaseConfigurationPath)
        {
            this._databaseConfiguration = ConfigurationHelper.Load<DatabaseConfiguration>(databaseConfigurationPath);
        }

        /// <summary>
        /// Creates a new <see cref="DatabaseContext"/>.
        /// </summary>
        /// <returns></returns>
        public DatabaseContext CreateDbContext() => new DatabaseContext(this._databaseConfiguration);

        /// <summary>
        /// Creates a new <see cref="DatabaseContext"/> by passing context options.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public DatabaseContext CreateDbContext(DbContextOptions<DatabaseContext> options) => new DatabaseContext(options);

        /// <summary>
        /// Creates a new <see cref="DatabaseContext"/>.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DatabaseContext CreateDbContext(string[] args)
        {
            if (args != null && args.Length > 0)
                this.Initialize(args[0]);

            return this.CreateDbContext();
        }
    }
}
