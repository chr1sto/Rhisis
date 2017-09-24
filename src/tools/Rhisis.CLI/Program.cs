using Microsoft.Extensions.CommandLineUtils;
using Rhisis.CLI.Commands;
using Rhisis.CLI.Interfaces;
using System;
using System.IO;
using System.Reflection;

namespace Rhisis.CLI
{
    public static class Program
    {
        private static readonly Assembly CurrentAssembly = Assembly.GetEntryAssembly();
        private static readonly string Version = CurrentAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        private static readonly ICommand[] Commands = new ICommand[]
        {
            new DatabaseCommand(),
            new ConfigureCommand()
        };

        public static void Main(string[] args)
        {
#if DEBUG
            // DEBUG: The following command line intializes the database witht the "database.json" file as configuration.
            // DEBUG: Rhisis.CLI.exe database initialize --configuration database.json
            args = BuildDebugArgs("database", "initialize");
#endif

            string appDescription = LoadResourceText("Rhisis.CLI.Resources.Description.txt");
            var app = new CommandLineApplication
            {
                Name = "Rhisis.CLI.exe",
                FullName = "Rhisis Command Line Interface",
                Description = appDescription
            };

            app.HelpOption("-?|-h|--help");
            app.VersionOption("--version", "1.0.0");

            foreach (ICommand command in Commands)
                app.Command(command.Name, command.Execute);

            try
            {
                app.Execute(args);
            }
            catch (CommandParsingException ex)
            {
                Console.WriteLine(ex.Message);
                app.ShowHelp();
            }

#if DEBUG
            Console.WriteLine("Done.");
            Console.ReadLine();
#endif
        }

        private static string LoadResourceText(string resourceName)
        {
            var resourceText = string.Empty;

            using (Stream stream = CurrentAssembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
                resourceText = reader.ReadToEnd();

            return resourceText;
        }

        private static string[] BuildDebugArgs(params string[] args)
        {
            return args;
        }
    }
}
