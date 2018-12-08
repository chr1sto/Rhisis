using Ether.Network.Packets;
using Ether.Network.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Cluster.ISC;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Network;
using Rhisis.Network.ISC.Structures;
using Rhisis.Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Cluster
{
    public class ClusterServer : NetServer<ClusterClient>, IClusterServer
    {
        private readonly ILogger<ClusterServer> _logger;
        private readonly ClusterConfiguration _clusterConfiguration;

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
        public ClusterServer(ILogger<ClusterServer> logger, ClusterConfiguration clusterConfiguration)
        {
            this._logger = logger;
            this._clusterConfiguration = clusterConfiguration;
            this.Configuration.Host = this._clusterConfiguration.Host;
            this.Configuration.Port = this._clusterConfiguration.Port;
            this.Configuration.MaximumNumberOfConnections = 1000;
            this.Configuration.Backlog = 100;
            this.Configuration.BufferSize = 4096;

            this._logger.LogTrace("Host: {0}, Port: {1}, MaxNumberOfConnections: {2}, Backlog: {3}, BufferSize: {4}",
                this.Configuration.Host,
                this.Configuration.Port,
                this.Configuration.MaximumNumberOfConnections,
                this.Configuration.Backlog,
                this.Configuration.BufferSize);
        }

        /// <inheritdoc />
        protected override void Initialize()
        {
            this._logger.LogInformation("Connection to ISC server on {0}:{1}...", this._clusterConfiguration.ISC.Host, this._clusterConfiguration.ISC.Port);
            InterClient = new ISCClient(this._clusterConfiguration);
            InterClient.Connect();

            //TODO: Implement this log inside OnStarted method when will be available.
            this._logger.LogInformation("'{0}' cluster server is started and listen on {1}:{2}.", 
                InterClient.ClusterConfiguration.Name, this.Configuration.Host, this.Configuration.Port);
        }

        /// <inheritdoc />
        protected override void OnClientConnected(ClusterClient client)
        {
            this._logger.LogInformation("New client connected from {0}.", client.RemoteEndPoint);

            CommonPacketFactory.SendWelcome(client, client.SessionId);
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(ClusterClient client)
        {
            this._logger.LogInformation("Client disconnected from {0}.", client.RemoteEndPoint);
        }

        /// <inheritdoc />
        protected override void OnError(Exception exception)
        {
            this._logger.LogInformation($"Socket error: {exception.Message}");
        }

        /// <inheritdoc />
        public WorldServerInfo GetWorldServerById(int id) => WorldServers.FirstOrDefault(x => x.Id == id);
    }
}
