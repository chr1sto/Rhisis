using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Sylver.HandlerInvoker;
using Sylver.Network.Data;
using Sylver.Network.Server;

namespace Rhisis.Cluster.WorldCluster
{
    public class WorldClusterServerClient : NetServerClient, IWorldClusterServerClient
    {
        private ILogger<WorldClusterServerClient> _logger;
        private IHandlerInvoker _handlerInvoker;
        
        public WorldClusterServerClient(Socket socketConnection) : base(socketConnection)
        {
        }
        
        /// <summary>
        /// Initialize the <see cref="WorldClusterServerClient"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="handlerInvoker">Handler Invoker.</param>
        public void Initialize(ILogger<WorldClusterServerClient> logger, IHandlerInvoker handlerInvoker)
        {
            _logger = logger;
            _handlerInvoker = handlerInvoker;
        }

        public override void HandleMessage(INetPacketStream packet)
        {
            throw new System.NotImplementedException();
        }
    }
}