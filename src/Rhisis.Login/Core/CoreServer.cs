using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Login.Core.Packets;
using Rhisis.Network.Core;
using Sylver.HandlerInvoker;
using Sylver.Network.Server;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Login.Core
{
    /// <summary>
    /// Defines the core server mechanism.
    /// </summary>
    internal class CoreServer : NetServer<CoreServerClient>, ICoreServer
    {
        private const int ClientBacklog = 50;
        private const int ClientBufferSize = 64;
        private readonly ILogger<CoreServer> _logger;
        private readonly CoreConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHandlerInvoker _handlerInvoker;

        /// <summary>
        /// Creates a new <see cref="CoreServer"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="configuration">Core server configuration.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public CoreServer(ILogger<CoreServer> logger, IOptions<CoreConfiguration> configuration, IServiceProvider serviceProvider, IHandlerInvoker handlerInvoker)
        {
            _logger = logger;
            _configuration = configuration.Value;
            _serviceProvider = serviceProvider;
            _handlerInvoker = handlerInvoker;
            ServerConfiguration = new NetServerConfiguration(_configuration.Host, 
                _configuration.Port, 
                ClientBacklog, 
                ClientBufferSize);
        }

        /// <inheritdoc />
        protected override void OnAfterStart()
        {
            _logger.LogInformation($"{nameof(CoreServer)} started!");
        }

        /// <inheritdoc />
        protected override void OnClientConnected(CoreServerClient connection)
        {
            var corePacketFactory = _serviceProvider.GetRequiredService<ICorePacketFactory>();

            connection.Initialize(_serviceProvider.GetRequiredService<ILogger<CoreServerClient>>(), _handlerInvoker);

            corePacketFactory.SendWelcome(connection);

            _logger.LogTrace($"New incoming Core client connection from {connection.Socket.RemoteEndPoint}.");
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(CoreServerClient connection)
        {
            _handlerInvoker.Invoke(CorePacketType.Disconnect, connection);
        }

        /// <inheritdoc />
        //protected override void OnError(Exception exception)
        //{
        //    this._logger.LogError(exception, $"An error occured in {nameof(CoreServer)}.");
        //}

        /// <inheritdoc />
        public CoreServerClient GetClusterServer(int clusterId)
        {
            return Clients.FirstOrDefault(x => x.ServerInfo is ClusterServerInfo cluster && cluster.Id == clusterId);
        }

        /// <inheritdoc />
        public bool HasCluster(int clusterId) => GetClusterServer(clusterId) != null;

        /// <inheritdoc />
        public CoreServerClient GetWorldServer(int parentClusterId, int worldId)
        {
            return Clients.FirstOrDefault(x => x.ServerInfo is WorldServerInfo world && 
                world.ParentClusterId == parentClusterId && world.Id == worldId);
        }

        /// <inheritdoc />
        public bool HasWorld(int parentClusterId, int worldId) => GetWorldServer(parentClusterId, worldId) != null;

        /// <inheritdoc />
        public IEnumerable<ClusterServerInfo> GetConnectedClusters() 
            => Clients.Where(x => x.ServerInfo.GetType() == typeof(ClusterServerInfo)).Select(x => x.ServerInfo as ClusterServerInfo);
    }
}
