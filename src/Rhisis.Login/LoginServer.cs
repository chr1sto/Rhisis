using Ether.Network;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Configuration;
using System;

namespace Rhisis.Login
{
    public sealed class LoginServer : NetServer<LoginClient>
    {
        private static readonly string LoginConfigFile = "config/login.json";

        public LoginConfiguration LoginConfiguration { get; private set; }

        public LoginServer()
        {
            Console.Title = "Rhisis - Login Server";
            this.LoadConfiguration();
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

        private void LoadConfiguration()
        {
            this.LoginConfiguration = ConfigurationHelper.Load<LoginConfiguration>(LoginConfigFile, true);

            this.Configuration.Host = this.LoginConfiguration.Host;
            this.Configuration.Port = this.LoginConfiguration.Port;
            this.Configuration.MaximumNumberOfConnections = 1000;
            this.Configuration.Backlog = 100;
            this.Configuration.BufferSize = 4096;
        }
    }
}