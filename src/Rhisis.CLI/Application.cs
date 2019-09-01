using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Commands.Configure;
using Rhisis.CLI.Commands.Database;
using Rhisis.CLI.Commands.Setup;
using Rhisis.CLI.Commands.User;

namespace Rhisis.CLI
{
    [Command(ThrowOnUnexpectedArgument = false, Description = Description)]
    //[Subcommand(typeof(DatabaseCommand))]
    //[Subcommand(typeof(SetupCommand))]
    //[Subcommand(typeof(UserCommand))]
    [Subcommand(typeof(ConfigureCommand))]
    public class Application
    {
        public const string DefaultDatabaseConfigurationFile = "config/database.json";
        public const string Description = "This tool is a command line interface allowing " +
                                          "administrators to manage their own servers easily.";
        
        public void OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
        }
    }
}
