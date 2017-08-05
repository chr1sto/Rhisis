namespace Rhisis.Login
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            using (var server = new LoginServer())
                server.Start();
        }
    }
}