using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Rhisis.Core.Helpers;
using Rhisis.Database.Context;

namespace Rhisis.Database
{
    public class DatabaseFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        private const string MigrationConfigurationEnv = "DB_CONFIG";
        
        /// <inheritdoc />
        public DatabaseContext CreateDbContext(string[] args)
        {
            var configurationPath = Environment.GetEnvironmentVariable(MigrationConfigurationEnv);
            var configuration = ConfigurationHelper.Load<DatabaseConfiguration>(configurationPath);

            if (configuration == null)
                throw new InvalidOperationException($"Cannot find database configuration path: '{configurationPath}'.");

            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.ConfigureCorrectDatabase(configuration);
            return new DatabaseContext(optionsBuilder.Options, configuration);
        }
        
        /// <summary>
        /// Creates a database on the given database configuration.
        /// Lifetime should be managed by IOC framework if using any!
        /// </summary>
        /// <param name="configuration">The database configuration to use</param>
        /// <returns>A new instance of an IDatabase implementation</returns>
        public IDatabase GetDatabase(DatabaseConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.ConfigureCorrectDatabase(configuration);
            var databaseContext = new DatabaseContext(optionsBuilder.Options, configuration);
            return new Database(databaseContext);
        }
    }
}