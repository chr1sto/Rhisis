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
            throw new NotImplementedException();
        }
    }
}
