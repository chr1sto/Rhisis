using System;
using Microsoft.Extensions.Logging;
using Rhisis.Network.Core;
using Rhisis.World.ClusterClient.Packets;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;

namespace Rhisis.World.ClusterClient.Handlers
{
    [Handler]
    public class ClusterHandler
    {
        private readonly ILogger<ClusterHandler> _logger;
        private readonly IClusterPacketFactory _clusterPacketFactory;

        /// <summary>
        /// Creates a new <see cref="ClusterHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="clusterPacketFactory">Core packet factory.</param>
        public ClusterHandler(ILogger<ClusterHandler> logger, IClusterPacketFactory clusterPacketFactory)
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
        public void OnWelcome(IWorldClusterClient client)
        {
            _clusterPacketFactory.SendAuthentication(client);
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
                    _logger.LogInformation("World server authenticated succesfully with parent cluster server.");
                    return;
                case CoreAuthenticationResultType.FailedUnknownServer:
                    _logger.LogCritical("Unable to authenticate world server. " +
                                        "Reason: world cluster server doesn't recognize this server. You probably have to update all servers.");
                    break;
                case CoreAuthenticationResultType.FailedWrongPassword:
                    _logger.LogCritical("Unable to authenticate world server. " +
                                        "Reason: wrong password.");
                    break;
                default:
                    _logger.LogTrace("Core authentification result: {0}", authenticationResult);
                    _logger.LogCritical("Unable to authenticate World Core client. " +
                                        "Reason: Cannot recognize cluster server. You probably have to update all servers.");
                    break;
            }

            if (Core.Common.OperatingSystem.IsWindows())
                Console.ReadLine();

            Environment.Exit((int)authenticationResult);
        }
    }
}
