using System;
using System.Globalization;
using NLog;
using Rhisis.Core.IO;

namespace Rhisis.World
{
    public static class Program
    {
        private const string ProgramTitle = "Rhisis - World Server";

        private static WorldServer _server;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static void Main()
        {
            Console.Title = ProgramTitle;
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            EncodingUtilities.Initialize();

            try
            {
                Logger.Info("Starting World server...");

                _server = new WorldServer();
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