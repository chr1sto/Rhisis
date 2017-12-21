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
            AssemblyLoadContext.Default.Unloading += Default_Unloading;

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

        private static void Default_Unloading(AssemblyLoadContext obj)
        {
            Server?.Dispose();
        }
    }
}