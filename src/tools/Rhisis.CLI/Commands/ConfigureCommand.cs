using Microsoft.Extensions.CommandLineUtils;
using Rhisis.CLI.Interfaces;
using System;

namespace Rhisis.CLI.Commands
{
    internal sealed class ConfigureCommand : ICommand
    {
        private static readonly string CommandName = "configure";
        private static readonly string CommandDescription = "Configure the Rhisis emulator.";

        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Name => CommandName;

        /// <summary>
        /// Gets the command description.
        /// </summary>
        public string Description => CommandDescription;

        /// <summary>
        /// Execute the command logic.
        /// </summary>
        public void Execute(CommandLineApplication command)
        {
            command.OnExecute(() =>
            {
                Console.WriteLine("Configure Rhisis command.");
                return 0;
            });
        }
    }
}
