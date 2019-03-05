using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Commands.Database;
using Rhisis.CLI.Commands.User;

namespace Rhisis.CLI
{
    [Command(ThrowOnUnexpectedArgument = false, Description = Program.Description)]
    [Subcommand(typeof(DatabaseCommand))]
    [Subcommand(typeof(UserCommand))]
    public class Application
    {
        public const string DefaultDatabaseConfigurationFile = "config/database.json";

        public void OnExecute(CommandLineApplication app, IConsole console)
        {
            app.ShowHelp();
        }
    }
}
