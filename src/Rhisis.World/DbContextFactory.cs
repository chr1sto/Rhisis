using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Rhisis.Core.Helpers;
using Rhisis.Database;
using Rhisis.Database.Context;

namespace Rhisis.Migrations
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
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
    }
}