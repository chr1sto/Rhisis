using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Rhisis.Core.Common;
using Rhisis.Core.Helpers;
using System;

namespace Rhisis.Database
{
    public sealed class DatabaseFactory : Singleton<DatabaseFactory>, IDesignTimeDbContextFactory<DatabaseContext>
    {
        private const string MigrationConfigurationEnv = "DB_CONFIG";

        public DatabaseConfiguration Configuration { get; private set; }

        /// <summary>
        /// Setup the <see cref="DatabaseFactory"/> and Data Access Layer assembly.
        /// </summary>
        public void Setup()
        {
            // Nothing to do for now. Just forcing the assembly loading.
        }

        /// <summary>
        /// Initialize the <see cref="DatabaseFactory"/>.
        /// </summary>
        /// <param name="databaseConfigurationPath"></param>
        public void Initialize(string databaseConfigurationPath) => 
            this.Configuration = ConfigurationHelper.Load<DatabaseConfiguration>(databaseConfigurationPath);

        /// <summary>
        /// Creates a new <see cref="DatabaseContext"/>.
        /// </summary>
        /// <returns></returns>
        public DatabaseContext CreateDbContext() => new DatabaseContext(this.Configuration);

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
            var test = Environment.GetEnvironmentVariable(MigrationConfigurationEnv);

            if (!string.IsNullOrEmpty(test))
                this.Initialize(test);

            return this.CreateDbContext();
        }

        /// <summary>
        /// Check if the database exists.
        /// </summary>
        /// <returns></returns>
        public bool DatabaseExists()
        {
            bool result = false;

            using (var dbContext = this.CreateDbContext())
            {
                result = dbContext.DatabaseExists();
            }

            return result;
        }
    }
}
