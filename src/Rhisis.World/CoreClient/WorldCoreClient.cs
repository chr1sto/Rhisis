using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Network.Core;
using Sylver.HandlerInvoker;
using Sylver.Network.Client;
using Sylver.Network.Data;
using System;

namespace Rhisis.World.CoreClient
{
    public sealed class WorldCoreClient : NetClient, IWorldCoreClient
    {
        public const int BufferSize = 128;

        private readonly ILogger<WorldCoreClient> _logger;
        private readonly IHandlerInvoker _handlerInvoker;

        /// <inheritdoc />
        public WorldConfiguration WorldServerConfiguration { get; }

        /// <inheritdoc />
        public CoreConfiguration CoreClientConfiguration { get; }

        /// <inheritdoc />
        public string RemoteEndPoint => this.Socket.RemoteEndPoint.ToString();

        /// <summary>
        /// Creates a new <see cref="WorldCoreClient"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="worldConfiguration">World server configuration.</param>
        /// <param name="coreConfiguration">Core server configuration.</param>
        /// <param name="handlerInvoker">Handler invoker.</param>
        public WorldCoreClient(ILogger<WorldCoreClient> logger, IOptions<WorldConfiguration> worldConfiguration, IOptions<CoreConfiguration> coreConfiguration, IHandlerInvoker handlerInvoker)
        {
            this._logger = logger;
            this.WorldServerConfiguration = worldConfiguration.Value;
            this.CoreClientConfiguration = coreConfiguration.Value;
            this._handlerInvoker = handlerInvoker;
            this.ClientConfiguration = new NetClientConfiguration(this.CoreClientConfiguration.Host, this.CoreClientConfiguration.Port, BufferSize);
        }

        /// <inheritdoc />
        protected override void OnConnected()
        {
            this._logger.LogInformation($"{nameof(WorldCoreClient)} connected to core server.");
        }

        /// <inheritdoc />
        protected override void OnDisconnected()
        {
            this._logger.LogInformation($"{nameof(WorldCoreClient)} disconnected from core server.");

            // TODO: try to reconnect.
        }

        /// <inheritdoc />
        //protected override void OnSocketError(SocketError socketError)
        //{
        //    this._logger.LogError($"An error occured on {nameof(WorldCoreClient)}: {socketError}");
        //}

        /// <inheritdoc />
        public override void HandleMessage(INetPacketStream packet)
        {
            uint packetHeaderNumber = 0;

            if (this.Socket == null)
            {
                this._logger.LogError($"Skip to handle core packet from server. Reason: {nameof(WorldCoreClient)} is not connected.");
                return;
            }

            try
            {
                packetHeaderNumber = packet.Read<uint>();
                this._handlerInvoker.Invoke((CorePacketType)packetHeaderNumber, this, packet);
            }
            catch (ArgumentNullException)
            {
                if (Enum.IsDefined(typeof(CorePacketType), packetHeaderNumber))
                    this._logger.LogWarning("Received an unimplemented Core packet {0} (0x{1}) from {2}.", Enum.GetName(typeof(CorePacketType), packetHeaderNumber), packetHeaderNumber.ToString("X4"), this.RemoteEndPoint);
                else
                    this._logger.LogWarning("[SECURITY] Received an unknown Core packet 0x{0} from {1}.", packetHeaderNumber.ToString("X4"), this.RemoteEndPoint);
            }
            catch (Exception exception)
            {
                this._logger.LogError(exception, $"An error occured while handling a core packet.");
                this._logger.LogDebug(exception.InnerException?.StackTrace);
            }
        }
    }
}
