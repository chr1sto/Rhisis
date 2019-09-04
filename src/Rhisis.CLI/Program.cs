using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        public static async Task<int> Main(string[] args)
        {
            return await new HostBuilder()
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddConsole();
                })
                .ConfigureServices((context, services) =>
                {
                    services.RegisterDatabaseFactory();
                    services.AddSingleton(PhysicalConsole.Singleton);
                    services.AddSingleton<ConsoleHelper>();
                })
                .RunCommandLineApplicationAsync<Application>(args);
        }
    }
}