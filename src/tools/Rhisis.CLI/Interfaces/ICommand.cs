using Microsoft.Extensions.CommandLineUtils;

namespace Rhisis.CLI.Interfaces
{
    /// <summary>
    /// Describes the behavior of a Rhisis CLI command.
    /// </summary>
    internal interface ICommand
    {
        /// <summary>
        /// Gets the command name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the command description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Executes the command logic.
        /// </summary>
        /// <param name="command">Command</param>
        void Execute(CommandLineApplication command);
    }
}
