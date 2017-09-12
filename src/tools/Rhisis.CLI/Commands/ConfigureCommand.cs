using Rhisis.CLI.Interfaces;

namespace Rhisis.CLI.Commands
{
    internal sealed class ConfigureCommand : ICommand
    {
        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Name => "configure";

        /// <summary>
        /// Gets the command description.
        /// </summary>
        public string Description => "Configure the Rhisis emulator.";

        /// <summary>
        /// Execute the command logic.
        /// </summary>
        public void Execute()
        {
        }
    }
}
