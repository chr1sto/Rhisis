using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.Network;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Database.Exceptions;
using System;
using System.Collections.Generic;

namespace Rhisis.Cluster
{
    public sealed class ClusterServer : NetServer<ClusterClient>
    {
        private static readonly string ClusterConfigFile = "config/cluster.json";
        private static readonly string DatabaseConfigFile = "config/database.json";

        public ClusterServer()
        {
            Console.Title = "Rhisis - Login Server";
            Logger.Initialize();
            this.LoadConfiguration();
        }

        protected override void Initialize()
        {
            PacketHandler<ClusterClient>.Initialize();

            var databaseConfiguration = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigFile, true);

            DatabaseService.Configure(databaseConfiguration);
            if (!DatabaseService.GetContext().DatabaseExists())
                throw new RhisisDatabaseException($"The database '{databaseConfiguration.Database}' doesn't exists yet.");
            
            // TODO: connect to ISC

            Logger.Info("Rhisis cluster server is up");
        }

        protected override void OnClientConnected(ClusterClient connection)
        {
            Logger.Info("New client connected: {0}", connection.Id);
        }

        protected override void OnClientDisconnected(ClusterClient connection)
        {
            Logger.Info("Client {0} disconnected.", connection.Id);
        }

        protected override IReadOnlyCollection<NetPacketBase> SplitPackets(byte[] buffer)
        {
            return FFPacket.SplitPackets(buffer);
        }

        private void LoadConfiguration()
        {
            var clusterConfiguration = ConfigurationHelper.Load<ClusterConfiguration>(ClusterConfigFile, true);

            this.Configuration.Host = clusterConfiguration.Host;
            this.Configuration.Port = clusterConfiguration.Port;
            this.Configuration.MaximumNumberOfConnections = 1000;
            this.Configuration.Backlog = 100;
            this.Configuration.BufferSize = 4096;
        }
    }
}
