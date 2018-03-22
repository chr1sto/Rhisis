using System;

namespace Rhisis.Cluster
{
    public static class Program
    {
        private const string ProgramTitle = "Rhisis - ClusterServer";

        private static void Main()
        {
            Console.Title = ProgramTitle;

            using (var server = new ClusterServer())
                server.Start();
        }
    }
}