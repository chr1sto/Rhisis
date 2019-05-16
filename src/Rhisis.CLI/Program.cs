using McMaster.Extensions.CommandLineUtils;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.Database;

namespace Rhisis.CLI
{
    public static class Program
    {
        public const string Description = "This tool is a command line interface allowing administrators to manage their own servers easily.";

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var dbConfig = ConfigurationHelper.Load<DatabaseConfiguration>(Application.DefaultDatabaseConfigurationFile);
            DependencyContainer.Instance
                .GetServiceCollection()
                .RegisterDatabaseServices(dbConfig);

            DependencyContainer.Instance.BuildServiceProvider();
                
            CommandLineApplication.Execute<Application>(args);
            
            DependencyContainer.Instance.Dispose();
        }
    }
}
