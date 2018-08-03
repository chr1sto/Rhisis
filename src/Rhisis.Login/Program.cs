using NLog;
using System;

namespace Rhisis.Login
{
    public static class Program
    {
        private const string ProgramTitle = "Rhisis - LoginServer";
        private static LoginServer _server;

        private static void Main()
        {
            Console.Title = ProgramTitle;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            try
            {
                _server = new LoginServer();
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