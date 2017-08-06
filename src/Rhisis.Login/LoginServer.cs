using Ether.Network;
using Rhisis.Core.Structures.Configuration;
using System;
using System.IO;

namespace Rhisis.Login
{
    public sealed class LoginServer : NetServer<LoginClient>
    {
        private static readonly string LoginConfigFile = "config/login.json";

        public LoginServer()
        {
            Console.Title = "Rhisis - Login Server";
        }

        protected override void Initialize()
        {
            Console.WriteLine("Server running state: {0}", this.IsRunning);
        }

        protected override void OnClientConnected(LoginClient connection)
        {
            Console.WriteLine("New client connected: {0}", connection.Id);
        }

        protected override void OnClientDisconnected(LoginClient connection)
        {
            Console.WriteLine("Client {0} disconnected.", connection.Id);
        }
    }
}