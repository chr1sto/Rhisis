using Ether.Network.Packets;
using Microsoft.Extensions.Logging;
using Rhisis.Cluster.CoreClient.Packets;
using Rhisis.Core.Handlers.Attributes;
using Rhisis.Network.Core;
using System;

namespace Rhisis.Cluster.CoreClient.Handlers
{
    [Handler]
    public sealed class CommonCoreHandlers
    {
        private readonly ILogger<CommonCoreHandlers> _logger;
        private readonly ICorePacketFactory _corePacketFactory;

        public CommonCoreHandlers(ILogger<CommonCoreHandlers> logger, ICorePacketFactory corePacketFactory)
        {
            this._logger = logger;
            this._corePacketFactory = corePacketFactory;
        }

        [HandlerAction(CorePacketType.Welcome)]
        public void OnWelcome(IClusterCoreClient client)
        {
            this._corePacketFactory.SendAuthentication(client);
        }

        [HandlerAction(CorePacketType.AuthenticationResult)]
        public void OnAuthenticationResult(IClusterCoreClient client, INetPacketStream packet)
        {
            var authenticationResult = (CoreAuthenticationResultType)(packet.Read<uint>());

            switch (authenticationResult)
            {
                case CoreAuthenticationResultType.Success:
                    this._logger.LogInformation("Cluster Core client authenticated succesfully.");
                    return;
                case CoreAuthenticationResultType.FailedClusterExists:
                    this._logger.LogCritical("Unable to authenticate Cluster Core client. Reason: an other Cluster server (with the same id) is already connected.");
                    break;
                case CoreAuthenticationResultType.FailedUnknownServer:
                    this._logger.LogCritical("Unable to authenticate Cluster Core client. Reason: ISC server doesn't recognize this server. You probably have to update all servers.");
                    break;
                default:
                    this._logger.LogTrace("Core authentification result: {0}", authenticationResult);
                    this._logger.LogCritical("Unable to authenticate ISC client. Reason: Cannot recognize ISC server. You probably have to update all servers.");
                    break;
            }

            //TODO: implement a peacefully shutdown.
            Console.ReadLine();
            Environment.Exit((int)authenticationResult);
        }
    }
}
