using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Commands.Game.Quests;

namespace Rhisis.CLI.Commands.Resources
{
    [Command("resources", Description = "Manages the database users")]
    [Subcommand(typeof(QuestCommand))]
    public class ResourcesCommand
    {
        public void OnExecute(CommandLineApplication app) => app.ShowHelp();
    }
}
