using Microsoft.Extensions.CommandLineUtils;
using Rhisis.CLI.Interfaces;

namespace Rhisis.CLI.Commands
{
    internal sealed class DatabaseCommand : ICommand
    {
        private static readonly string CommandName = "database";
        private static readonly string CommandDescription = "Database management";
        private readonly ICommand[] _subCommands = new ICommand[]
        {
            new DatabaseInitializeCommand(),
            new DatabaseUpdateCommand()
        };

        /// <summary>
        /// Gets the command name.
        /// </summary>
        public string Name => CommandName;

        /// <summary>
        /// Gets the command description.
        /// </summary>
        public string Description => CommandDescription;

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
