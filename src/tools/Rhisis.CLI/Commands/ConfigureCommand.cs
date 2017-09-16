using Microsoft.Extensions.CommandLineUtils;
using Rhisis.CLI.Interfaces;
using System;

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
        public void Execute(CommandLineApplication command)
        {
            throw new NotImplementedException();
        }
    }
}
