using System;
using System.Globalization;
using NLog;
using Rhisis.Core.IO;

namespace Rhisis.World
{
    public static class Program
    {
        private const string ProgramTitle = "Rhisis - WorldServer";
        private static WorldServer _server;

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
                _server = new WorldServer();
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