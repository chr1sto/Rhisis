using System;

namespace Rhisis.World
{
    public static class Program
    {
        private static readonly string ProgramTitle = "Rhisis - WorldServer";

        private static void Main(string[] args)
        {
            Console.Title = ProgramTitle;

            using (var server = new WorldServer())
                server.Start();
        }
    }
}