using System;
using System.Globalization;

namespace Rhisis.World
{
    public static class Program
    {
        private static readonly string ProgramTitle = "Rhisis - WorldServer";

        private static void Main(string[] args)
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