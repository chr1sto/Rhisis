using Microsoft.Extensions.CommandLineUtils;
using Rhisis.CLI.Commands;
using Rhisis.CLI.Interfaces;
using System;
using System.Reflection;

namespace Rhisis.CLI
{
    public class Program
    {
        private static readonly string Version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        private static readonly ICommand[] Commands = new ICommand[]
        {
            new DatabaseCommand(),
            new ConfigureCommand()
        };

        public static void Main(string[] args)
        {
            var app = new CommandLineApplication()
            {
                Name = "Rhisis.CLI.exe",
                Description = "Rhisis CLI (Command Line Interface)"
            };

            app.HelpOption("-?|-h|--help");
            app.VersionOption("--version", Version);

            foreach (ICommand command in Commands)
                app.Command(command.Name, command.Execute);

            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 0;
            });

            try
            {
                app.Execute(args);
            }
            catch (CommandParsingException ex)
            {
                Console.WriteLine(ex.Message);
                app.ShowHelp();
            }
        }
    }
}
