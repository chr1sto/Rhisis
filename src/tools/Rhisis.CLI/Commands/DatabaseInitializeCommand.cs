using Microsoft.Extensions.CommandLineUtils;
using Rhisis.CLI.Interfaces;
using Rhisis.Database;
using System;

namespace Rhisis.CLI.Commands
{
    internal sealed class DatabaseInitializeCommand : ICommand
    {
        /// <summary>
        /// Gets the command's name.
        /// </summary>
        public string Name => "initialize";

        /// <summary>
        /// Gets the command's description.
        /// </summary>
        public string Description => "Initialize the database.";

        /// <summary>
        /// Executes the command's logic.
        /// </summary>
        /// <param name="command">Command</param>
        public void Execute(CommandLineApplication command)
        {
            command.HelpOption("-?|-h|--help");
            command.Description = this.Description;

            var opt = command.Option("-c|--configuration", "Sets the database configuration file.", CommandOptionType.SingleValue);

            command.OnExecute(() =>
            {
                string databaseConfiguration = opt.HasValue() ? opt.Value() : "config/database.json";

                DatabaseService.Configure(null);
                using (var rhisisDbContext = DatabaseService.GetContext())
                {
                    if (!rhisisDbContext.DatabaseExists())
                        rhisisDbContext.CreateDatabase();
                }

                return 0;
            });
        }
    }
}
