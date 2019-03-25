using Ether.Network.Client;
using Ether.Network.Packets;
using NLog;
using Rhisis.Core.Exceptions;
using Rhisis.Network.ISC.Packets;
using Rhisis.Network.ISC.Structures;
using Rhisis.Network;
using Rhisis.Core.Structures.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Rhisis.Cluster.ISC
{
    public sealed class ISCClient : NetClient
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets the cluster server configuration.
        /// </summary>
        public ClusterConfiguration ClusterConfiguration { get; }

        /// <summary>
        /// Gets the world server's informations connected to this cluster.
        /// </summary>
        public IList<WorldServerInfo> WorldServers { get; }

        /// <summary>
        /// Gets the remote end point (IP and port) for this client.
        /// </summary>
        public string RemoteEndPoint { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ISCClient"/> instance.
        /// </summary>
        /// <param name="configuration">Cluster Server configuration</param>
        public ISCClient(ClusterConfiguration configuration)
        {
            this.ClusterConfiguration = configuration;
            this.WorldServers = new List<WorldServerInfo>();
            this.Configuration.Host = this.ClusterConfiguration.ISC.Host;
            this.Configuration.Port = this.ClusterConfiguration.ISC.Port;
            this.Configuration.BufferSize = 512;

            Logger.Trace("ISC config -> Host: {0}, Port: {1}, BufferSize: {2}",
                this.Configuration.Host,
                this.Configuration.Port,
                this.Configuration.BufferSize);
        }

        /// <inheritdoc />
        public override void Send(INetPacketStream packet)
        {
            if (Logger.IsTraceEnabled)
                Logger.Trace("Send {0} packet to server.",(ISCPacketType)BitConverter.ToUInt32(packet.Buffer, 4));

            base.Send(packet);
        }

        /// <inheritdoc />
        public override void HandleMessage(INetPacketStream packet)
        {
            uint packetHeaderNumber = 0;

            if (Socket == null)
            {
                Logger.Error("Skip to handle packet from server. Reason: socket is no more connected.");
                return;
            }

            try
            {
                packetHeaderNumber = packet.Read<uint>();

                if (Logger.IsTraceEnabled)
                    Logger.Trace("Received {0} packet from server.", (ISCPacketType)packetHeaderNumber);

                PacketHandler<ISCClient>.Invoke(this, packet, (ISCPacketType)packetHeaderNumber);
            }
            catch (KeyNotFoundException)
            {
                Logger.Warn("[SECURITY] Received an unknown ISC packet header 0x{0} from server.", packetHeaderNumber.ToString("X2"));
            }
            catch (RhisisPacketException packetException)
            {
                Logger.Error("ISC packet handle error from server. {0}", packetException);
                Logger.Debug(packetException.InnerException?.StackTrace);
            }
        }

        /// <inheritdoc />
        protected override void OnConnected()
        {
            this.RemoteEndPoint = this.Socket.RemoteEndPoint.ToString();
            Logger.Debug("ISC client connected to {0}.", this.RemoteEndPoint);
        }

        /// <inheritdoc />
        protected override void OnDisconnected()
        {
            Logger.Error("Disconnected from ISC server.");
            //TODO: implement reconnection, otherwise FATAL error + stop server.
        }

        /// <inheritdoc />
        protected override void OnSocketError(SocketError socketError)
        {
            Logger.Error("ISC socket error: {0}", socketError);
        }
    }
}
