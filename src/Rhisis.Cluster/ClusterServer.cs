using Ether.Network.Packets;
using Ether.Network.Server;
using NLog;
using Rhisis.Cluster.ISC;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Network;
using Rhisis.Network.ISC.Structures;
using Rhisis.Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Cluster
{
    public sealed class ClusterServer : NetServer<ClusterClient>
    {
        private const string ClusterConfigFile = "config/cluster.json";
        private const string DatabaseConfigFile = "config/database.json";

        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets the ISC client.
        /// </summary>
        public static ISCClient InterClient { get; private set; }

        /// <summary>
        /// Gets the list of the connected world servers of this cluster.
        /// </summary>
        public static IReadOnlyCollection<WorldServerInfo> WorldServers => InterClient.WorldServers as IReadOnlyCollection<WorldServerInfo>;

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
            this.LoadConfiguration();
        }

        /// <summary>
        /// Load the cluster server's configuration.
        /// </summary>
        private void LoadConfiguration()
        {
            Logger.Debug("Loading server configuration from '{0}'...", ClusterConfigFile);
            this.ClusterConfiguration = ConfigurationHelper.Load<ClusterConfiguration>(ClusterConfigFile, true);

            this.Configuration.Host = this.ClusterConfiguration.Host;
            this.Configuration.Port = this.ClusterConfiguration.Port;
            this.Configuration.MaximumNumberOfConnections = 1000;
            this.Configuration.Backlog = 100;
            this.Configuration.BufferSize = 4096;

            Logger.Trace("Host: {0}, Port: {1}, MaxNumberOfConnections: {2}, Backlog: {3}, BufferSize: {4}",
                this.Configuration.Host,
                this.Configuration.Port,
                this.Configuration.MaximumNumberOfConnections,
                this.Configuration.Backlog,
                this.Configuration.BufferSize);
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            PacketHandler<ISCClient>.Initialize();
            PacketHandler<ClusterClient>.Initialize();

            Logger.Debug("Loading database configuration from '{0}'...", DatabaseConfigFile);
            DatabaseFactory.Instance.Initialize(DatabaseConfigFile);
            Logger.Trace($"Database config -> {DatabaseFactory.Instance.Configuration}");
            
            DependencyContainer.Instance.Initialize().BuildServiceProvider();

            Logger.Info("Connection to ISC server on {0}:{1}...", this.ClusterConfiguration.ISC.Host, this.ClusterConfiguration.ISC.Port);
            InterClient = new ISCClient(this.ClusterConfiguration);
            InterClient.Connect();

            //TODO: Implement this log inside OnStarted method when will be available.
            Logger.Info("'{0}' cluster server is started and listen on {1}:{2}.", 
                InterClient.ClusterConfiguration.Name, this.Configuration.Host, this.Configuration.Port);
        }

        /// <inheritdoc />
        protected override void OnClientConnected(ClusterClient client)
        {
            client.Initialize(this);
            Logger.Info("New client connected from {0}.", client.RemoteEndPoint);
            CommonPacketFactory.SendWelcome(client, client.SessionId);
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(ClusterClient client)
        {
            Logger.Info("Client disconnected from {0}.", client.RemoteEndPoint);
        }

        /// <inheritdoc />
        protected override void OnError(Exception exception)
        {
            Logger.Error($"Socket error: {exception.Message}");
        }

        /// <summary>
        /// Gets world server by his id.
        /// </summary>
        /// <param name="id">World Server id</param>
        /// <returns></returns>
        public static WorldServerInfo GetWorldServerById(int id) => WorldServers.FirstOrDefault(x => x.Id == id);
    }
}
