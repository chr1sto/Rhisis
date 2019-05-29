using System;
using McMaster.Extensions.CommandLineUtils;
using Rhisis.Core.Helpers;
using Rhisis.Database;

namespace Rhisis.CLI.Commands.Database
{
    [Command("update", Description = "Updates the database structure")]
    public class DatabaseUpdateCommand
    {
        private readonly IDatabase _database;

        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration",
            Description = "Specify the database configuration file path.")]
        public string DatabaseConfigurationFile { get; set; }

        public DatabaseUpdateCommand(DatabaseFactory databaseFactory)
        {
            if (string.IsNullOrEmpty(DatabaseConfigurationFile))
                DatabaseConfigurationFile = Application.DefaultDatabaseConfigurationFile;

            var dbConfig = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigurationFile);
            _database = databaseFactory.GetDatabase(dbConfig);
        }

        public void OnExecute()
        {
            try
            {
                Console.WriteLine("Starting database structure update...");
                var rhisisDbContext = _database.DatabaseContext;
                if (rhisisDbContext.DatabaseExists())
                {
                    rhisisDbContext.Migrate();
                    Console.WriteLine("Database updated.");
                }
                else
                {
                    Console.WriteLine("Database does not exist yet!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}