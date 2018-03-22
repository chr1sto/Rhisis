using Ether.Network.Server;
using Rhisis.Cluster.ISC;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.ISC.Structures;
using Rhisis.Core.Network;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Database.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Ether.Network.Packets;

namespace Rhisis.Cluster
{
    public sealed class ClusterServer : NetServer<ClusterClient>
    {
        private const string ClusterConfigFile = "config/cluster.json";
        private const string DatabaseConfigFile = "config/database.json";
        private static ISCClient _client;

        /// <summary>
        /// Gets the InterClient.
        /// </summary>
        public static ISCClient InterClient => _client;

        /// <summary>
        /// Gets the list of the connected world servers of this cluster.
        /// </summary>
        public static IReadOnlyCollection<WorldServerInfo> Worlds => InterClient.Worlds as IReadOnlyCollection<WorldServerInfo>;

        /// <summary>
        /// Gets the cluster server's configuration.
        /// </summary>
        public ClusterConfiguration ClusterConfiguration { get; private set; }

        /// <inheritdoc />
        protected override IPacketProcessor PacketProcessor { get; } = new FlyffPacketProcessor();

        /// <summary>
        /// Creates a new <see cref="ClusterServer"/> instance.
        /// </summary>
        public ClusterServer()
        {
            Logger.Initialize("cluster");
            this.LoadConfiguration();
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            PacketHandler<ClusterClient>.Initialize();
            PacketHandler<ISCClient>.Initialize();

            var databaseConfiguration = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigFile, true);

            DatabaseService.Configure(databaseConfiguration);
            if (!DatabaseService.GetContext().DatabaseExists())
                throw new RhisisDatabaseException($"The database '{databaseConfiguration.Database}' doesn't exists yet.");

            _client = new ISCClient(this.ClusterConfiguration);
            _client.Connect();

            Logger.Info("Rhisis cluster server is up");
        }

        /// <inheritdoc />
        protected override void OnClientConnected(ClusterClient connection)
        {
            Logger.Info("New client connected: {0}", connection.Id);
            connection.InitializeClient(this);
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(ClusterClient connection)
        {
            Logger.Info("Client {0} disconnected.", connection.Id);
        }

        /// <inheritdoc />
        protected override void OnError(Exception exception)
        {
            Logger.Error("ClusterServer Error: {0}", exception.Message);
        }

        /// <summary>
        /// Load the cluster server's configuration.
        /// </summary>
        private void LoadConfiguration()
        {
            this.ClusterConfiguration = ConfigurationHelper.Load<ClusterConfiguration>(ClusterConfigFile, true);

            this.Configuration.Host = this.ClusterConfiguration.Host;
            this.Configuration.Port = this.ClusterConfiguration.Port;
            this.Configuration.MaximumNumberOfConnections = 1000;
            this.Configuration.Backlog = 100;
            this.Configuration.BufferSize = 4096;
        }

        /// <summary>
        /// Gets world server by his id.
        /// </summary>
        /// <param name="id">World Server id</param>
        /// <returns></returns>
        public static WorldServerInfo GetWorldServerById(int id) => Worlds.FirstOrDefault(x => x.Id == id);
    }
}
