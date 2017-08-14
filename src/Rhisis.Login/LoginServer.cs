using Ether.Network;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Structures.Configuration;
using System;
using Ether.Network.Packets;
using System.Collections.Generic;
using Rhisis.Database;

namespace Rhisis.Login
{
    public sealed class LoginServer : NetServer<LoginClient>
    {
        private static readonly string LoginConfigFile = "config/login.json";
        private static readonly string DatabaseConfigFile = "config/database.json";
        
        public LoginConfiguration LoginConfiguration { get; private set; }

        public LoginServer()
        {
            Console.Title = "Rhisis - Login Server";
            Logger.Initialize();
            this.LoadConfiguration();
        }

        protected override void Initialize()
        {
            FFPacketHandler<LoginClient>.Initialize();
            this.InitializeDatabase();

            Console.WriteLine("Server running state: {0}", this.IsRunning);
        }

        protected override void OnClientConnected(LoginClient connection)
        {
            Console.WriteLine("New client connected: {0}", connection.Id);
            connection.InitializeClient(this.LoginConfiguration);
        }

        protected override void OnClientDisconnected(LoginClient connection)
        {
            Console.WriteLine("Client {0} disconnected.", connection.Id);
        }

        protected override IReadOnlyCollection<NetPacketBase> SplitPackets(byte[] buffer)
        {
            return FFPacket.SplitPackets(buffer);
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

        private void InitializeDatabase()
        {
            var databaseConfiguration = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigFile, true);

            DatabaseService.Configure(databaseConfiguration);

            using (var dbContext = DatabaseService.GetContext())
            {
                var exists = dbContext.DatabaseExists();

                Console.WriteLine("Database exists: {0}", exists);

                if (!exists)
                    dbContext.CreateDatabase();

                exists = dbContext.DatabaseExists();

                Console.WriteLine("Database exists: {0}", exists);
            }
        }
    }
}