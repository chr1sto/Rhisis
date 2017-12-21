using System;
using System.Runtime.Loader;

namespace Rhisis.Login
{
    public static class Program
    {
        private static readonly string ProgramTitle = "Rhisis - LoginServer";
        private static LoginServer Server = null;

        private static void Main(string[] args)
        {
            Console.Title = ProgramTitle;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            try
            {
                Server = new LoginServer();
                Server.Start();
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
                Server.Dispose();
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Server?.Dispose();
        }
    }
}