using McMaster.Extensions.CommandLineUtils;
using Rhisis.Core.Helpers;
using Rhisis.Database;
using System;

namespace Rhisis.CLI.Commands
{
    [Command(Name = "database", Description = "Manages the database")]
    [Subcommand("configure", typeof(DatabaseConfigureCommand))]
    [Subcommand("update", typeof(DatabaseUpdateCommand))]
    public class DatabaseCommand
    {
        public void OnExecute(CommandLineApplication app, IConsole console)
        {
            app.ShowHelp();
        }
    }

    [Command(Name = "configure", Description = "Configures the database access")]
    public class DatabaseConfigureCommand
    {
        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration", Description = "Specify the database configuration file path.")]
        public string DatabaseConfigurationFile { get; set; }

        public void OnExecute(CommandLineApplication app, IConsole console)
        {
            if (string.IsNullOrEmpty(DatabaseConfigurationFile))
                DatabaseConfigurationFile = "config/database.json";

            var dbConfiguration = new DatabaseConfiguration();

            while ((dbConfiguration.Provider = GetDatabaseProvider()) == DatabaseProvider.Unknown) ;

            Console.Write("Host: ");
            dbConfiguration.Host = Console.ReadLine();

            Console.Write("Username: ");
            dbConfiguration.Username = Console.ReadLine();

            Console.Write("Password: ");
            dbConfiguration.Password = Console.ReadLine();

            Console.Write("Database name: ");
            dbConfiguration.Database = Console.ReadLine();

            Console.WriteLine("--------------------------------");
            Console.WriteLine("Configuration:");
            Console.WriteLine($"Database Provider: {dbConfiguration.Provider.ToString()}");
            Console.WriteLine($"Host: {dbConfiguration.Host}");
            Console.WriteLine($"Username: {dbConfiguration.Username}");
            Console.WriteLine($"Database name: {dbConfiguration.Database}");
            Console.WriteLine("--------------------------------");

            Console.WriteLine("Save this configuration ? (y/n)");
            string response = Console.ReadLine();

            if (response.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                ConfigurationHelper.Save(DatabaseConfigurationFile, dbConfiguration);
                Console.WriteLine($"Database configuration saved in '{DatabaseConfigurationFile}'.");
            }
        }

        private DatabaseProvider GetDatabaseProvider()
        {
            var databaseProvider = DatabaseProvider.Unknown;

            Console.WriteLine("Select one of the available providers:");
            string[] providerNames = Enum.GetNames(typeof(DatabaseProvider));
            int[] providerValues = (int[])Enum.GetValues(typeof(DatabaseProvider));

            for (int i = 1; i < providerNames.Length; i++)
                Console.WriteLine($"{providerValues[i]}. {providerNames[i]}");

            Console.Write("Database provider: ");
            string selectedProvider = Console.ReadLine();

            if (int.TryParse(selectedProvider, out int selectedProviderValue) && selectedProviderValue > 0 && selectedProviderValue < providerValues.Length)
                databaseProvider = (DatabaseProvider)selectedProviderValue;
            else if (Enum.TryParse(selectedProvider, true, out DatabaseProvider provider))
                databaseProvider = provider;

            if (databaseProvider == DatabaseProvider.Unknown)
                Console.WriteLine($"Invalid database provider: {selectedProvider}.");

            return databaseProvider;
        }
    }

    [Command(Name = "update", Description = "Updates the database structure")]
    public class DatabaseUpdateCommand
    {
        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration", Description = "Specify the database configuration file path.")]
        public string DatabaseConfigurationFile { get; set; }

        public void OnExecute(CommandLineApplication app, IConsole console)
        {
            if (string.IsNullOrEmpty(DatabaseConfigurationFile))
                DatabaseConfigurationFile = "config/database.json";

            try
            {
                Console.WriteLine("Starting database structure update...");
                var databaseConfiguration = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigurationFile);

                DatabaseService.Configure(databaseConfiguration);
                using (var rhisisDbContext = DatabaseService.GetContext())
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
