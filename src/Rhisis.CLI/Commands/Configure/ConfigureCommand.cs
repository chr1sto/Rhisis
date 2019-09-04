using McMaster.Extensions.CommandLineUtils;

namespace Rhisis.CLI.Commands.Configure
{
    [Command("configure", Description = "Configures a Rhisis login server.")]
    [Subcommand(typeof(LoginServerConfigurationCommand))]
    [Subcommand(typeof(ClusterServerConfigurationCommand))]
    [Subcommand(typeof(WorldServerConfigurationCommand))]
    public class ConfigureCommand
    {
        public void OnExecute(CommandLineApplication app) => app.ShowHelp();
    }
}
