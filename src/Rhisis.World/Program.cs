using System;

namespace Rhisis.World
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            using (var server = new WorldServer())
                server.Start();
        }
    }
}