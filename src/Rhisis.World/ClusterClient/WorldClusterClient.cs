using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Network.Core;
using Sylver.HandlerInvoker;
using Sylver.Network.Client;
using Sylver.Network.Data;

namespace Rhisis.World.ClusterClient
{
    public sealed class WorldClusterClient : NetClient, IWorldClusterClient
    {
        public const int BufferSize = 128;

        private readonly ILogger<WorldClusterClient> _logger;
        private readonly IHandlerInvoker _handlerInvoker;

        /// <inheritdoc />
        public WorldConfiguration WorldServerConfiguration { get; }

        /// <inheritdoc />
        public WorldClusterConfiguration WorldClusterClientConfiguration { get; }

        /// <inheritdoc />
        public string RemoteEndPoint => Socket.RemoteEndPoint.ToString();

        /// <summary>
        /// Creates a new <see cref="WorldClusterClient"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="worldConfiguration">World server configuration.</param>
        /// <param name="worldClusterConfiguration">World cluster server configuration.</param>
        /// <param name="handlerInvoker">Handler invoker.</param>
        public WorldClusterClient(ILogger<WorldClusterClient> logger, IOptions<WorldConfiguration> worldConfiguration, 
            IOptions<WorldClusterConfiguration> worldClusterConfiguration, IHandlerInvoker handlerInvoker)
        {
            _logger = logger;
            WorldServerConfiguration = worldConfiguration.Value;
            WorldClusterClientConfiguration = worldClusterConfiguration.Value;
            _handlerInvoker = handlerInvoker;
            ClientConfiguration = new NetClientConfiguration(WorldClusterClientConfiguration.Host, WorldClusterClientConfiguration.Port, BufferSize);
        }

        /// <inheritdoc />
        protected override void OnConnected()
        {
            _logger.LogInformation($"{nameof(WorldClusterClient)} connected to world cluster server.");
        }

        /// <inheritdoc />
        protected override void OnDisconnected()
        {
            _logger.LogInformation($"{nameof(WorldClusterClient)} disconnected from world cluster  server.");
        }

        /// <inheritdoc />
        public override void HandleMessage(INetPacketStream packet)
        {
            uint packetHeaderNumber = 0;

            if (Socket == null)
            {
                _logger.LogError($"Skip to handle core packet from server. Reason: {nameof(WorldClusterClient)} is not connected.");
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
                    _logger.LogWarning("Received an unimplemented Core packet {0} (0x{1}) from {2}.", Enum.GetName(typeof(CorePacketType), packetHeaderNumber), packetHeaderNumber.ToString("X4"), RemoteEndPoint);
                else
                    _logger.LogWarning("[SECURITY] Received an unknown Core packet 0x{0} from {1}.", packetHeaderNumber.ToString("X4"), RemoteEndPoint);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occured while handling a core packet.");
                _logger.LogDebug(exception.InnerException?.StackTrace);
            }
        }
    }
}
