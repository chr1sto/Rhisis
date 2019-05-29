using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Services;
using Rhisis.Database;

namespace Rhisis.CLI.Commands.Database
{
    [Command("initialize", Description = "Configures and initialize the database.")]
    public class DatabaseInitializationCommand
    {
        private readonly DatabaseConfigureCommand _configurationCommand;
        private readonly DatabaseUpdateCommand _updateCommand;
        
        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration", Description = "Specify the database configuration file path.")]
        public string DatabaseConfigurationFile { get; set; }

        public DatabaseInitializationCommand(DatabaseFactory databaseFactory, ConsoleHelper consoleHelper)
        {
            this._configurationCommand = new DatabaseConfigureCommand(consoleHelper)
            {
                DatabaseConfigurationFile = this.DatabaseConfigurationFile
            };
            this._updateCommand = new DatabaseUpdateCommand(databaseFactory)
            {
                DatabaseConfigurationFile = this.DatabaseConfigurationFile
            };
        }

        public void OnExecute()
        {
            this._configurationCommand.OnExecute();
            this._updateCommand.OnExecute();
        }
    }
}
