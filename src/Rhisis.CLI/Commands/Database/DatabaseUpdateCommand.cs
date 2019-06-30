using System;
using McMaster.Extensions.CommandLineUtils;
using Rhisis.Core.Helpers;
using Rhisis.Database;
using Rhisis.Database.Context;

namespace Rhisis.CLI.Commands.Database
{
    [Command("update", Description = "Updates the database structure")]
    public class DatabaseUpdateCommand
    {
        private readonly DatabaseFactory _databaseFactory;

        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration",
            Description = "Specify the database configuration file path.")]
        public string DatabaseConfigurationFile { get; set; }

        public DatabaseUpdateCommand(DatabaseFactory databaseFactory)
        {
            this._databaseFactory = databaseFactory;
        }

        public void OnExecute()
        {
            try
            {
                if (string.IsNullOrEmpty(this.DatabaseConfigurationFile))
                    this.DatabaseConfigurationFile = Application.DefaultDatabaseConfigurationFile;

                DatabaseConfiguration dbConfig = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigurationFile);

                using (IDatabase database = this._databaseFactory.GetDatabase(dbConfig))
                {
                    Console.WriteLine("Starting database structure update...");
                    DatabaseContext rhisisDbContext = database.DatabaseContext;

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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}