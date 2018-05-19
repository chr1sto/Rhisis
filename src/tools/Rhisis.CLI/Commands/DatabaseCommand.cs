using McMaster.Extensions.CommandLineUtils;
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
        public void OnExecute(CommandLineApplication app, IConsole console)
        {
            DatabaseProvider provider = DatabaseProvider.Unknown;

            while ((provider = GetDatabaseProvider()) == DatabaseProvider.Unknown) ;
            Console.WriteLine($"Selected provider: {provider.ToString()}");
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
        public void OnExecute(CommandLineApplication app, IConsole console)
        {
            Console.WriteLine("Database update");
        }
    }
}
