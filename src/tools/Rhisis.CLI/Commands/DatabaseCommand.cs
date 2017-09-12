using Rhisis.CLI.Interfaces;
using System;

namespace Rhisis.CLI.Commands
{
    internal sealed class DatabaseCommand : ICommand
    {
        private static readonly string InitializeCommandName = "initialize";
        private static readonly string UpdateCommandName = "update";

        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Name => "database";

        /// <summary>
        /// Gets the command description.
        /// </summary>
        public string Description => "Database management";

        /// <summary>
        /// Executes the command logic.
        /// </summary>
        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
