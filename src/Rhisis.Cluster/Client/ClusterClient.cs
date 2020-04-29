using Microsoft.Extensions.Logging;
using Rhisis.Cluster.Packets;
using Rhisis.Core.Helpers;
using Rhisis.Network.Packets;
using Sylver.HandlerInvoker;
using Sylver.Network.Data;
using Sylver.Network.Server;
using System;
using System.Net.Sockets;

namespace Rhisis.Cluster.Client
{
    public sealed class ClusterClient : NetServerClient, IClusterClient
    {
        private ILogger<ClusterClient> _logger;
        private IClusterServer _clusterServer;
        private IHandlerInvoker _handlerInvoker;

        /// <inheritdoc />
        public uint SessionId { get; }

        /// <inheritdoc />
        public int LoginProtectValue { get; set; }

        /// <summary>
        /// Creates a new <see cref="ClusterClient"/> instance.
        /// </summary>
        /// <param name="socketConnection">Socket connection.</param>
        public ClusterClient(Socket socketConnection)
            : base(socketConnection)
        {
            SessionId = RandomHelper.GenerateSessionKey();
            LoginProtectValue = new Random().Next(0, 1000);
        }

        /// <summary>
        /// Initializes the <see cref="ClusterClient"/>.
        /// </summary>
        /// <param name="clusterServer">Parent cluster server.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="handlerInvoker">Handler invoker.</param>
        /// <param name="clusterPacketFactory">Cluster packet factory.</param>
        public void Initialize(IClusterServer clusterServer, ILogger<ClusterClient> logger, IHandlerInvoker handlerInvoker)
        {
            _clusterServer = clusterServer;
            _logger = logger;
            _handlerInvoker = handlerInvoker;
        }

        /// <inheritdoc />
        public void Disconnect()
        {
            Dispose();
            _clusterServer.DisconnectClient(Id);
        }

        /// <summary>
        /// Send a packet to the client.
        /// </summary>
        /// <param name="packet"></param>
        public override void Send(INetPacketStream packet)
        {
            _logger.LogTrace("Send {0} packet to {1}.",
                    (PacketType)BitConverter.ToUInt32(packet.Buffer, 5),
                    Socket.RemoteEndPoint);

            base.Send(packet);
        }

        /// <summary>
        /// Handle the incoming mesages.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public override void HandleMessage(INetPacketStream packet)
        {
            uint packetHeaderNumber = 0;

            if (Socket == null)
            {
                _logger.LogTrace("Skip to handle cluster packet from null socket. Reason: client is not connected.");
                return;
            }

            try
            {
                packet.Read<uint>(); // DPID: Always 0xFFFFFFFF (uint.MaxValue)
                packetHeaderNumber = packet.Read<uint>();

#if DEBUG
                _logger.LogTrace("Received {0} packet from {1}.", (PacketType)packetHeaderNumber, Socket.RemoteEndPoint);
#endif
                _handlerInvoker.Invoke((PacketType)packetHeaderNumber, this, packet);
            }
            catch (ArgumentNullException)
            {
                if (Enum.IsDefined(typeof(PacketType), packetHeaderNumber))
                    _logger.LogWarning("Received an unimplemented Cluster packet {0} (0x{1}) from {2}.", Enum.GetName(typeof(PacketType), packetHeaderNumber), packetHeaderNumber.ToString("X4"), Socket.RemoteEndPoint);
                else
                    _logger.LogWarning("[SECURITY] Received an unknown Cluster packet 0x{0} from {1}.", packetHeaderNumber.ToString("X4"), Socket.RemoteEndPoint);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occured while handling a cluster packet.");
                _logger.LogDebug(exception.InnerException?.StackTrace);
            }
        }
    }
}
