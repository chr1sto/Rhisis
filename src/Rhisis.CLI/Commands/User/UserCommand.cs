using McMaster.Extensions.CommandLineUtils;

namespace Rhisis.CLI.Commands.User
{
    [Command("user", Description = "Manages the database users")]
    [Subcommand(typeof(UserCreateCommand))]
    [Subcommand(typeof(UserShowCommand))]
    public sealed class UserCommand
    {
        public void OnExecute(CommandLineApplication app, IConsole console) => app.ShowHelp();
    }
}
