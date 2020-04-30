using Microsoft.Extensions.Logging;
using Rhisis.Cluster.WorldCluster.Packets;
using Rhisis.Network.Core;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;

namespace Rhisis.Cluster.WorldCluster.Handlers
{
    [Handler]
    internal sealed class WorldHandler
    {
        private readonly ILogger<WorldHandler> _logger;
        private readonly IWorldClusterServer _worldClusterServer;
        private readonly IWorldPacketFactory _worldPacketFactory;

        /// <summary>
        /// Creates a new <see cref="WorldHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="worldClusterServer">Core server instance.</param>
        /// <param name="worldPacketFactory">Core server packet factory.</param>
        public WorldHandler(ILogger<WorldHandler> logger, IWorldClusterServer worldClusterServer, IWorldPacketFactory worldPacketFactory)
        {
            _logger = logger;
            _worldClusterServer = worldClusterServer;
            _worldPacketFactory = worldPacketFactory;
        }
        
        [HandlerAction(WorldClusterPacketType.Authenticate)]
        public void OnAuthentication(IWorldClusterServerClient client, INetPacketStream _)
        {
            _worldPacketFactory.SendAuthenticationResult(client, CoreAuthenticationResultType.Success);
        }
    }
}