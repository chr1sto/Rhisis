using Microsoft.Extensions.CommandLineUtils;
using Rhisis.CLI.Interfaces;
using Rhisis.Core.Helpers;
using Rhisis.Database;
using System;
using System.IO;

namespace Rhisis.CLI.Commands
{
    internal sealed class DatabaseInitializeCommand : ICommand
    {
        private static readonly string CommandName = "initialize";
        private static readonly string CommandDescription = "Initialize the database.";

        /// <summary>
        /// Gets the command's name.
        /// </summary>
        public string Name => CommandName;

        /// <summary>
        /// Gets the command's description.
        /// </summary>
        public string Description => CommandDescription;

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
                try
                {
                    var databaseConfigurationFile = opt.HasValue() ? opt.Value() : "config/database.json";
                    var databaseConfiguration = ConfigurationHelper.Load<DatabaseConfiguration>(databaseConfigurationFile, true);

                    DatabaseService.Configure(databaseConfiguration);
                    using (var rhisisDbContext = DatabaseService.GetContext())
                    {
                        rhisisDbContext.Migrate();
                    }
                }
                catch (FileNotFoundException fileNotFound)
                {
                    Console.WriteLine($"{fileNotFound.Message} - {fileNotFound.FileName}");
                    return -1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return -1;
                }

                return 0;
            });
        }
    }
}
