using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Reflection;

namespace Rhisis.CLI
{
    public class Program
    {
        private static readonly string Version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        public static void Main(string[] args)
        {
            var app = new CommandLineApplication()
            {
                Name = "Rhisis.CLI.exe",
                FullName = "Rhisis CLI",
                Description = "Rhisis CLI (Command Line Interface)"
            };

            app.HelpOption("-?|-h|--help");
            app.VersionOption("--version", Version);

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
