using NLog;
using System;
using Rhisis.Core.IO;

namespace Rhisis.Cluster
{
    public static class Program
    {
        private const string Title = "Rhisis - Cluster Server";

        private static ClusterServer _server;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static void Main()
        {
            Console.Title = Title;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            EncodingUtilities.Initialize();

            try
            {
                Logger.Info("Starting Cluster server...");

                _server = new ClusterServer();
                _server.Start();
            }
            catch (Exception e)
            {
                Logger.Fatal(e);
                Console.ReadLine();
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            _server?.Dispose();
            LogManager.Shutdown();
        }
    }
}