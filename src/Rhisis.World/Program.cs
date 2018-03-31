using System;
using System.Globalization;

namespace Rhisis.World
{
    public static class Program
    {
        private const string ProgramTitle = "Rhisis - WorldServer";

        private static void Main()
        {
            Console.Title = ProgramTitle;
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            using (var server = new WorldServer())
                server.Start();
        }
    }
}