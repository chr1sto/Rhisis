using Ether.Network.Packets;
using Ether.Network.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Cluster.Client;
using Rhisis.Cluster.Packets;
using Rhisis.Core.Handlers;
using Rhisis.Core.Resources;
using Rhisis.Core.Resources.Loaders;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Network;
using Rhisis.Network.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Cluster
{
    /// <summary>
    /// Cluster server.
    /// </summary>
    public class ClusterServer : NetServer<ClusterClient>, IClusterServer
    {
        private readonly ILogger<ClusterServer> _logger;
        private readonly IGameResources _gameResources;
        private readonly IServiceProvider _serviceProvider;

        /// <inheritdoc />
        public ClusterConfiguration ClusterConfiguration { get; }

        /// <inheritdoc />
        public IList<WorldServerInfo> WorldServers { get; } = new List<WorldServerInfo>();

        /// <inheritdoc />
        protected override IPacketProcessor PacketProcessor { get; } = new FlyffPacketProcessor();

        /// <summary>
        /// Creates a new <see cref="ClusterServer"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="clusterConfiguration">Cluster Server configuration.</param>
        /// <param name="gameResources">Game resources.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public ClusterServer(ILogger<ClusterServer> logger, IOptions<ClusterConfiguration> clusterConfiguration, IGameResources gameResources, IServiceProvider serviceProvider)
        {
            this._logger = logger;
            this.ClusterConfiguration = clusterConfiguration.Value;
            this._gameResources = gameResources;
            this._serviceProvider = serviceProvider;
            this.Configuration.Host = this.ClusterConfiguration.Host;
            this.Configuration.Port = this.ClusterConfiguration.Port;
            this.Configuration.MaximumNumberOfConnections = 1000;
            this.Configuration.Backlog = 100;
            this.Configuration.BufferSize = 4096;
            this.Configuration.Blocking = false;

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
            this._gameResources.Load(typeof(DefineLoader), typeof(JobLoader));

            //TODO: Implement this log inside OnStarted method when will be available.
            this._logger.LogInformation("'{0}' cluster server is started and listen on {1}:{2}.", 
                this.ClusterConfiguration.Name, this.Configuration.Host, this.Configuration.Port);
        }

        /// <inheritdoc />
        protected override void OnClientConnected(ClusterClient client)
        {
            this._logger.LogInformation("New client connected from {0}.", client.RemoteEndPoint);

            client.Initialize(this,
                this._serviceProvider.GetRequiredService<ILogger<ClusterClient>>(),
                this._serviceProvider.GetRequiredService<IHandlerInvoker>(),
                this._serviceProvider.GetRequiredService<IClusterPacketFactory>());
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(ClusterClient client)
        {
            this._logger.LogInformation("Client disconnected from {0}.", client.RemoteEndPoint);
        }

        /// <inheritdoc />
        protected override void OnError(Exception exception) => this._logger.LogInformation($"Socket error: {exception.Message}");

        /// <inheritdoc />
        public WorldServerInfo GetWorldServerById(int id) => this.WorldServers.FirstOrDefault(x => x.Id == id);
    }
}
