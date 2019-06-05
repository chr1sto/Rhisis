using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.CLI.Services;
using Rhisis.Database;

namespace Rhisis.CLI
{
    public static class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args"></param>
        public static int Main(string[] args)
        {
            var services = new ServiceCollection()
                .RegisterDatabaseFactory()
                .AddSingleton<ConsoleHelper>()
                .AddSingleton(PhysicalConsole.Singleton);
            
            using (var diContext = services.BuildServiceProvider())
            {
                var application = new CommandLineApplication<Application>();
                application.Conventions
                    .UseDefaultConventions()
                    .UseConstructorInjection(diContext);
                
                return application.Execute(args);    
            }
        }
    }
}