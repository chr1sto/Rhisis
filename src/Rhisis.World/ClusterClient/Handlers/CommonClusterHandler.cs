using System;
using Microsoft.Extensions.Logging;
using Rhisis.Network.Core;
using Rhisis.World.ClusterClient.Packets;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;

namespace Rhisis.World.ClusterClient.Handlers
{
    [Handler]
    public class CommonClusterHandler
    {
        private readonly ILogger<CommonClusterHandler> _logger;
        private readonly IClusterPacketFactory _clusterPacketFactory;

        /// <summary>
        /// Creates a new <see cref="CommonClusterHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="clusterPacketFactory">Core packet factory.</param>
        public CommonClusterHandler(ILogger<CommonClusterHandler> logger, IClusterPacketFactory clusterPacketFactory)
        {
            _logger = logger;
            _clusterPacketFactory = clusterPacketFactory;
        }

        /// <summary>
        /// Handles the welcome packet.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(CorePacketType.Welcome)]
        public void OnWelcome(IWorldClusterClient client, INetPacketStream _)
        {
            _clusterPacketFactory.SendAuthentication(client, client.WorldServerConfiguration);
        }

        /// <summary>
        /// Handles the core server authentication result.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(CorePacketType.AuthenticationResult)]
        public void OnAuthenticationResult(IWorldClusterClient _, INetPacketStream packet)
        {
            var authenticationResult = (CoreAuthenticationResultType)(packet.Read<uint>());

            switch (authenticationResult)
            {
                case CoreAuthenticationResultType.Success:
                    _logger.LogInformation("World Core client authenticated succesfully.");
                    return;
                case CoreAuthenticationResultType.FailedClusterExists:
                    _logger.LogCritical("Unable to authenticate World Core client. Reason: an other Cluster server (with the same id) is already connected.");
                    break;
                case CoreAuthenticationResultType.FailedUnknownServer:
                    _logger.LogCritical("Unable to authenticate World Core client. Reason: ISC server doesn't recognize this server. You probably have to update all servers.");
                    break;
                default:
                    _logger.LogTrace("Core authentification result: {0}", authenticationResult);
                    _logger.LogCritical("Unable to authenticate World Core client. Reason: Cannot recognize Core server. You probably have to update all servers.");
                    break;
            }

            if (Core.Common.OperatingSystem.IsWindows())
                Console.ReadLine();

            Environment.Exit((int)authenticationResult);
        }
    }
}
