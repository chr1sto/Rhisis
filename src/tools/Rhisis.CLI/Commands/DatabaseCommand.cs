using Microsoft.Extensions.CommandLineUtils;
using Rhisis.CLI.Interfaces;

namespace Rhisis.CLI.Commands
{
    internal sealed class DatabaseCommand : ICommand
    {
        private readonly ICommand[] _subCommands = new ICommand[]
        {
            new DatabaseInitializeCommand(),
            new DatabaseUpdateCommand()
        };

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
        public void Execute(CommandLineApplication command)
        {
            command.HelpOption("-?|-h|--help");
            command.Description = this.Description;

            foreach (ICommand subCommand in this._subCommands)
                command.Command(subCommand.Name, subCommand.Execute);
        }
    }
}
