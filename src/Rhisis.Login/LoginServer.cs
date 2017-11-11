using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.ISC.Structures;
using Rhisis.Core.Network;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Database.Exceptions;
using Rhisis.Login.ISC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhisis.Login
{
    public sealed class LoginServer : NetServer<LoginClient>
    {
        private static readonly string LoginConfigFile = "config/login.json";
        private static readonly string DatabaseConfigFile = "config/database.json";

        private ISCServer _interServer;
        
        public LoginConfiguration LoginConfiguration { get; private set; }

        public IEnumerable<ClusterServerInfo> ClustersConnected => this._interServer?.Clusters;

        public LoginServer()
        {
            Console.Title = "Rhisis - Login Server";
            Logger.Initialize();
            this.LoadConfiguration();
        }

        protected override void Initialize()
        {
            PacketHandler<LoginClient>.Initialize();
            
            var databaseConfiguration = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigFile, true);

            DatabaseService.Configure(databaseConfiguration);
            if (!DatabaseService.GetContext().DatabaseExists())
                throw new RhisisDatabaseException($"The database '{databaseConfiguration.Database}' doesn't exists yet.");

            Task.Run(() =>
            {
                this._interServer = new ISCServer(this.LoginConfiguration.ISC);
                this._interServer.Start();
            });

            Logger.Info("Rhisis login server is up");
        }

        protected override void OnClientConnected(LoginClient connection)
        {
            Logger.Info("New client connected: {0}", connection.Id);
            connection.InitializeClient(this);
        }

        protected override void OnClientDisconnected(LoginClient connection)
        {
            Logger.Info("Client {0} disconnected.", connection.Id);
        }

        protected override IReadOnlyCollection<NetPacketBase> SplitPackets(byte[] buffer)
        {
            return FFPacket.SplitPackets(buffer);
        }

        protected override void Dispose(bool disposing)
        {
            this._interServer.Stop();
            this._interServer.Dispose();

            base.Dispose(disposing);
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