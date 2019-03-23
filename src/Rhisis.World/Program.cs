using Rhisis.Core;

namespace Rhisis.World
{
    public static class Program
    {
        private static void Main()
        {
            ConsoleAppBootstrapper.CreateApp()
                .SetConsoleTitle("Rhisis - World Server")
                .SetCulture("en-US")
                .UseStartup<WorldServerStartup>()
                .Run();
        }
    }
}