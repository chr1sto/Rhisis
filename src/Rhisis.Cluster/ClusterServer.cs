using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Cluster.Client;
using Rhisis.Cluster.Packets;
using Rhisis.Core.Resources;
using Rhisis.Core.Resources.Loaders;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Network;
using Sylver.HandlerInvoker;
using Sylver.Network.Server;

namespace Rhisis.Cluster
{
    /// <summary>
    /// Cluster server.
    /// </summary>
    public class ClusterServer : NetServer<ClusterClient>, IClusterServer
    {
        private const int ClientBufferSize = 128;
        private const int ClientBacklog = 50;
        private readonly ILogger<ClusterServer> _logger;
        private readonly IGameResources _gameResources;
        private readonly IServiceProvider _serviceProvider;
        private readonly IRhisisDatabase _database;

        /// <inheritdoc />
        public ClusterConfiguration ClusterConfiguration { get; }

        /// <summary>
        /// Creates a new <see cref="ClusterServer"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="clusterConfiguration">Cluster Server configuration.</param>
        /// <param name="gameResources">Game resources.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public ClusterServer(ILogger<ClusterServer> logger, IOptions<ClusterConfiguration> clusterConfiguration, IGameResources gameResources, IServiceProvider serviceProvider, IRhisisDatabase database)
        {
            _logger = logger;
            ClusterConfiguration = clusterConfiguration.Value;
            _gameResources = gameResources;
            _serviceProvider = serviceProvider;
            _database = database;
            PacketProcessor = new FlyffPacketProcessor();
            ServerConfiguration = new NetServerConfiguration(ClusterConfiguration.Host,
                ClusterConfiguration.Port,
                ClientBacklog,
                ClientBufferSize);
        }

        /// <inheritdoc />
        protected override void OnBeforeStart()
        {
            if (!_database.IsAlive())
            {
                throw new InvalidProgramException($"Cannot start {nameof(ClusterServer)}. Failed to reach database.");
            }

            _gameResources.Load(typeof(DefineLoader), typeof(JobLoader));
        }

        /// <inheritdoc />
        protected override void OnAfterStart()
        {
            _logger.LogInformation($"'{ClusterConfiguration.Name}' cluster server is started and listening on {ServerConfiguration.Host}:{ServerConfiguration.Port}.");
        }

        /// <inheritdoc />
        protected override void OnClientConnected(ClusterClient client)
        {
            _logger.LogInformation($"New client connected to {nameof(ClusterServer)} from {client.Socket.RemoteEndPoint}.");

            client.Initialize(this,
                _serviceProvider.GetRequiredService<ILogger<ClusterClient>>(),
                _serviceProvider.GetRequiredService<IHandlerInvoker>());

            var clusterPacketFactory =  _serviceProvider.GetRequiredService<IClusterPacketFactory>();
            clusterPacketFactory.SendWelcome(client);
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(ClusterClient client)
        {
            _logger.LogInformation($"Client disconnected from {client.Socket.RemoteEndPoint}.");
        }
    }
}
