using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Network.Core;
using Sylver.HandlerInvoker;
using Sylver.Network.Client;
using Sylver.Network.Data;
using System;
using System.Collections.Generic;

namespace Rhisis.Cluster.CoreClient
{
    public sealed class ClusterCoreClient : NetClient, IClusterCoreClient
    {
        private const int BufferSize = 64;

        private readonly ILogger<ClusterCoreClient> _logger;
        private readonly CoreConfiguration _coreConfiguration;
        private readonly IHandlerInvoker _handlerInvoker;

        /// <inheritdoc />
        public ClusterConfiguration ClusterConfiguration { get; }

        /// <inheritdoc />
        public IList<WorldServerInfo> WorldServers { get; }

        /// <summary>
        /// Creates a new <see cref="ClusterCoreClient"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="handlerInvoker">Handler invoker.</param>
        /// <param name="clusterConfiguration">Cluster server configuration.</param>
        /// <param name="coreConfiguration">Core server configuration.</param>
        public ClusterCoreClient(ILogger<ClusterCoreClient> logger, IHandlerInvoker handlerInvoker, IOptions<ClusterConfiguration> clusterConfiguration, IOptions<CoreConfiguration> coreConfiguration)
        {
            _logger = logger;
            _handlerInvoker = handlerInvoker;
            _coreConfiguration = coreConfiguration.Value;
            ClusterConfiguration = clusterConfiguration.Value;
            ClientConfiguration = new NetClientConfiguration(_coreConfiguration.Host, _coreConfiguration.Port, BufferSize);
            WorldServers = new List<WorldServerInfo>();
        }

        /// <inheritdoc />
        protected override void OnConnected()
        {
            _logger.LogInformation("Cluster core client connected to core server.");
        }

        /// <inheritdoc />
        protected override void OnDisconnected()
        {
            _logger.LogInformation("Disconnected from core server.");

            // TODO: try to reconnect.
        }

        /// <inheritdoc />
        //protected override void OnSocketError(SocketError socketError)
        //{
        //    this._logger.LogError($"An error occured on Cluster core client: {socketError}");
        //}

        /// <inheritdoc />
        public override void HandleMessage(INetPacketStream packet)
        {
            uint packetHeaderNumber = 0;

            if (Socket == null)
            {
                _logger.LogError("Skip to handle core packet from server. Reason: socket is not connected.");
                return;
            }

            try
            {
                packetHeaderNumber = packet.Read<uint>();
                _handlerInvoker.Invoke((CorePacketType)packetHeaderNumber, this, packet);
            }
            catch (ArgumentNullException)
            {
                if (Enum.IsDefined(typeof(CorePacketType), packetHeaderNumber))
                    _logger.LogWarning("Received an unimplemented Core packet {0} (0x{1}) from {2}.", Enum.GetName(typeof(CorePacketType), packetHeaderNumber), packetHeaderNumber.ToString("X4"), Socket.RemoteEndPoint);
                else
                    _logger.LogWarning("[SECURITY] Received an unknown Core packet 0x{0} from {1}.", packetHeaderNumber.ToString("X4"), Socket.RemoteEndPoint);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occured while handling a core packet.");
                _logger.LogDebug(exception.InnerException?.StackTrace);
            }
        }
    }
}
