using Rhisis.Core;

namespace Rhisis.Cluster
{
    public static class Program
    {
        private static void Main()
        {
            ConsoleAppBootstrapper.CreateApp()
                .SetConsoleTitle("Rhisis - Cluster Server")
                .SetCulture("en-US")
                .UseStartup<ClusterServerStartup>()
                .Run();
        }
    }
}