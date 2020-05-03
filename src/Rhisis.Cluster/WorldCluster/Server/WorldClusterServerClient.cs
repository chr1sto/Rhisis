using System;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Rhisis.Network.Core;
using Sylver.HandlerInvoker;
using Sylver.Network.Data;
using Sylver.Network.Server;

namespace Rhisis.Cluster.WorldCluster.Server
{
    public class WorldClusterServerClient : NetServerClient, IWorldClusterServerClient
    {
        private ILogger<WorldClusterServerClient> _logger;
        private IHandlerInvoker _handlerInvoker;
        
        public WorldServerInfo ServerInfo { get; set; }
        
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
            uint packetHeaderNumber = 0;

            if (Socket == null)
            {
                _logger.LogTrace("Skip to handle world cluster packet from null socket. Reason: socket is not connected.");
                return;
            }

            try
            {
                packetHeaderNumber = packet.Read<uint>();
                _handlerInvoker.Invoke((CorePacketType)packetHeaderNumber, this, packet);
            }
            catch (ArgumentException)
            {
                if (Enum.IsDefined(typeof(CorePacketType), packetHeaderNumber))
                {
                    _logger.LogWarning("Received an unimplemented world cluster packet {0} (0x{1}) from {2}.",
                        Enum.GetName(typeof(CorePacketType), packetHeaderNumber),
                        packetHeaderNumber.ToString("X2"),
                        Socket.RemoteEndPoint);
                }
                else
                {
                    _logger.LogWarning($"Received an unknown world cluster packet 0x{packetHeaderNumber.ToString("X2")} from {Socket.RemoteEndPoint}.");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occured while handling a world cluster packet.");
                _logger.LogDebug(exception.InnerException?.StackTrace);
            }
        }
    }
}