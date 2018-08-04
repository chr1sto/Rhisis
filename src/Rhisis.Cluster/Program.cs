using NLog;
using System;
using Rhisis.Core.IO;

namespace Rhisis.Cluster
{
    public static class Program
    {
        private const string ProgramTitle = "Rhisis - ClusterServer";
        private static ClusterServer _server;

        private static void Main()
        {
            Console.Title = ProgramTitle;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            EncodingUtilities.Initialize();

            try
            {
                _server = new ClusterServer();
                _server.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
#if DEBUG
                Console.WriteLine(e.StackTrace);
#endif
            }
            finally
            {
                _server.Dispose();
                LogManager.Shutdown();
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            _server?.Dispose();
            LogManager.Shutdown();
        }
    }
}