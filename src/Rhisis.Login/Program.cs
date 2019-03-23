using Rhisis.Core;

namespace Rhisis.Login
{
    public static class Program
    {
        private static void Main()
        {
            ConsoleAppBootstrapper.CreateApp()
                .SetConsoleTitle("Rhisis - Login Server")
                .SetCulture("en-US")
                .UseStartup<LoginServerStartup>()
                .Run();
        }
    }
}