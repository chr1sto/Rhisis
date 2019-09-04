using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Services;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;

namespace Rhisis.CLI.Commands.Database
{
    [Command("initialize", Description = "Configures and initialize the database.")]
    public class DatabaseInitializationCommand
    {
        private readonly DatabaseConfigureCommand _configurationCommand;
        private readonly DatabaseUpdateCommand _updateCommand;

        /// <summary>
        /// Gets or sets the database configuration file.
        /// </summary>
        /// <remarks>
        /// If the database configuration file is not specified, the <see cref="ConfigurationConstants.DatabasePath"/> constant is used instead.
        /// </remarks>
        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration", Description = "Specify the database configuration file path.")]
        public string DatabaseConfigurationFile { get; set; }

        /// <summary>
        /// Creates a new <see cref="DatabaseInitializationCommand"/> instance.
        /// </summary>
        /// <param name="databaseFactory">Database factory.</param>
        /// <param name="consoleHelper">Console helper.</param>
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

        /// <summary>
        /// Executes the "database initialize" command.
        /// </summary>
        public void OnExecute()
        {
            this._configurationCommand.OnExecute();
            this._updateCommand.OnExecute();
        }
    }
}
