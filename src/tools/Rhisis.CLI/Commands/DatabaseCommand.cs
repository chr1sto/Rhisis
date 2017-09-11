using Microsoft.Extensions.CommandLineUtils;
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
        /// <param name="command"></param>
        public void Execute(CommandLineApplication command)
        {
            var initializeCommand = command.Argument(InitializeCommandName, "Creates and initialize the database");
            var updateCommand = command.Argument(UpdateCommandName, "Updates the database structure without data loss.");
            var configurationOption = command.Option("-c|--configuration", "Specify the database configuration file.", CommandOptionType.SingleValue);

            command.Description = this.Description;
            command.HelpOption("-?|-h|--help");
            
            command.OnExecute(() =>
            {
                if (InitializeCommandName.Equals(initializeCommand.Value, StringComparison.OrdinalIgnoreCase))
                    this.ProcessInitialize(configurationOption);
                else if (UpdateCommandName.Equals(updateCommand.Value, StringComparison.OrdinalIgnoreCase))
                    this.ProcessUpdate(configurationOption);
                else
                    command.ShowHelp();
                
                return 0;
            });
        }

        private void ProcessInitialize(CommandOption option)
        {
            Console.WriteLine("Process 'Initialize' command with opton: {0}", option.Value());
        }

        private void ProcessUpdate(CommandOption option)
        {
            Console.WriteLine("Process 'Update' command with opton: {0}", option.Value());
        }
    }
}
