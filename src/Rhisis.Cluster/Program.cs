using System;

namespace Rhisis.Cluster
{
    public static class Program
    {
        private static readonly string ProgramTitle = "Rhisis - ClusterServer";

        private static void Main(string[] args)
        {
            Console.Title = ProgramTitle;

            using (var server = new ClusterServer())
                server.Start();
        }
    }
}