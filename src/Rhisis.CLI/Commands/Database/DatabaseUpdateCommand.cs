using McMaster.Extensions.CommandLineUtils;
using Rhisis.Database;
using System;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.Database.Context;

namespace Rhisis.CLI.Commands.Database
{
    [Command("update", Description = "Updates the database structure")]
    public class DatabaseUpdateCommand
    {
        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration", Description = "Specify the database configuration file path.")]
        public string DatabaseConfigurationFile { get; set; }

        public void OnExecute(CommandLineApplication app, IConsole console)
        {
            if (string.IsNullOrEmpty(DatabaseConfigurationFile))
                DatabaseConfigurationFile = Application.DefaultDatabaseConfigurationFile;

            try
            {
                Console.WriteLine("Starting database structure update...");
                
                var dbConfig = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigurationFile);
                DependencyContainer.Instance
                    .GetServiceCollection()
                    .AddDbContext<DatabaseContext>(options => options.ConfigureCorrectDatabase(dbConfig));
//                DatabaseFactory.Instance.Initialize();

                using (var rhisisDbContext = DependencyContainer.Instance.Resolve<DatabaseContext>())
                {
                    if (rhisisDbContext.DatabaseExists())
                    {
                        rhisisDbContext.Migrate();
                        Console.WriteLine("Database updated.");
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
