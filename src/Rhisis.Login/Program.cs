using NLog;
using Rhisis.Core.IO;
using System;

namespace Rhisis.Login
{
    public static class Program
    {
        private const string Title = "Rhisis - Login Server";

        private static LoginServer _server;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static void Main()
        {
            Console.Title = Title;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            EncodingUtilities.Initialize();

            try
            {
                Logger.Info("Starting Login server...");

                _server = new LoginServer();
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