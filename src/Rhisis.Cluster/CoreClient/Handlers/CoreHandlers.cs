using Microsoft.Extensions.Logging;
using Rhisis.Cluster.CoreClient.Packets;
using Rhisis.Network.Core;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;
using System;
using Rhisis.Cluster.WorldCluster.Server;
using Rhisis.Core.Resources;

namespace Rhisis.Cluster.CoreClient.Handlers
{
    [Handler]
    public sealed class CoreHandlers
    {
        private readonly ILogger<CoreHandlers> _logger;
        private readonly ICorePacketFactory _corePacketFactory;
        private readonly IClusterServer _clusterServer;
        private readonly ICache<int, WorldServerInfo> _worldCache;

        /// <summary>
        /// Creates a new <see cref="CoreHandlers"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="corePacketFactory">Core packet factory.</param>
        /// <param name="clusterServer">Cluster server instance.</param>
        /// <param name="worldCache">Stores world server information</param>
        public CoreHandlers(ILogger<CoreHandlers> logger, ICorePacketFactory corePacketFactory,
            IClusterServer clusterServer, ICache<int, WorldServerInfo> worldCache) {
            _logger = logger;
            _corePacketFactory = corePacketFactory;
            _clusterServer = clusterServer;
            _worldCache = worldCache;
        }

        /// <summary>
        /// On welcome packet received.
        /// </summary>
        /// <param name="client"></param>
        [HandlerAction(CorePacketType.Welcome)]
        public void OnWelcome(IClusterCoreClient client)
        {
            _corePacketFactory.SendAuthentication(client, _clusterServer.ClusterConfiguration);
        }

        /// <summary>
        /// Handles the authentication result to the core server.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [HandlerAction(CorePacketType.AuthenticationResult)]
        public void OnAuthenticationResult(IClusterCoreClient client, INetPacketStream packet)
        {
            var authenticationResult = (CoreAuthenticationResultType)(packet.Read<uint>());

            switch (authenticationResult)
            {
                case CoreAuthenticationResultType.Success:
                {
                    _logger.LogInformation("Cluster Core client authenticated successfully.");
                    _corePacketFactory.SendUpdateWorldList(client, _worldCache.Items);
                    return;
                }
                case CoreAuthenticationResultType.FailedClusterExists:
                    _logger.LogCritical("Unable to authenticate Cluster Core client. Reason: an other Cluster server (with the same id) is already connected.");
                    break;
                case CoreAuthenticationResultType.FailedUnknownServer:
                    _logger.LogCritical("Unable to authenticate Cluster Core client. Reason: ISC server doesn't recognize this server. You probably have to update all servers.");
                    break;
                default:
                    _logger.LogTrace("Cluster core authentication result: {0}", authenticationResult);
                    _logger.LogCritical("Unable to authenticate Cluster Core client. Reason: Cannot recognize Core server. You probably have to update all servers.");
                    break;
            }

            if (Core.Common.OperatingSystem.IsWindows())
                Console.ReadLine();

            Environment.Exit((int)authenticationResult);
        }
    }
}
