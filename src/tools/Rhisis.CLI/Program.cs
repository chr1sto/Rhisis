using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Commands;

namespace Rhisis.CLI
{
    public static class Program
    {
        public const string Description = "This tool is a command line interface allowing user to manage their own servers easily.";

        public static void Main(string[] args) => CommandLineApplication.Execute<Application>(args);
    }

    [Command(ThrowOnUnexpectedArgument = false, Description = Program.Description)]
    [Subcommand("database", typeof(DatabaseCommand))]
    public class Application
    {
        public void OnExecute(CommandLineApplication app, IConsole console)
        {
            app.ShowHelp();
        }
    }
}
