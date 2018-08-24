using Ether.Network.Common;
using Ether.Network.Packets;
using NLog;
using Rhisis.Core.Exceptions;
using Rhisis.Core.Helpers;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Structures.Configuration;
using System;
using System.Collections.Generic;

namespace Rhisis.Cluster
{
    public sealed class ClusterClient : NetUser
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private ClusterServer _clusterServer;

        /// <summary>
        /// Gets the ID assigned to this session.
        /// </summary>
        public uint SessionId { get; }

        /// <summary>
        /// Gets the cluster server's configuration.
        /// </summary>
        public ClusterConfiguration Configuration => this._clusterServer.ClusterConfiguration;

        /// <summary>
        /// Gets or sets the Login protect value. 
        /// This value is random and valid only for this session in order to secure num pad disposition.
        /// </summary>
        public int LoginProtectValue { get; set; }

        /// <summary>
        /// Gets the remote end point (IP and port) for this client.
        /// </summary>
        public string RemoteEndPoint { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ClusterClient"/> instance.
        /// </summary>
        public ClusterClient()
        {
            this.SessionId = RandomHelper.GenerateSessionKey();
            this.LoginProtectValue = new Random().Next(0, 1000);
        }

        /// <summary>
        /// Initialize the <see cref="ClusterClient"/>.
        /// </summary>
        /// <param name="clusterServer"></param>
        public void Initialize(ClusterServer clusterServer)
        {
            this._clusterServer = clusterServer;
            this.RemoteEndPoint = this.Socket.RemoteEndPoint.ToString();
        }

        /// <summary>
        /// Disconnects the current <see cref="ClusterClient"/>.
        /// </summary>
        public void Disconnect()
        {
            this.Dispose();
            this._clusterServer.DisconnectClient(this.Id);
        }

        public override void Send(INetPacketStream packet)
        {
            if (Logger.IsTraceEnabled)
            {
                Logger.Trace("Send {0} packet to {1}.",
                    (PacketType)BitConverter.ToUInt32(packet.Buffer, 5),
                    this.RemoteEndPoint);
            }

            base.Send(packet);
        }

        /// <summary>
        /// Handle the incoming mesages.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public override void HandleMessage(INetPacketStream packet)
        {
            FFPacket pak = null;
            uint packetHeaderNumber = 0;

            if (Socket == null)
            {
                Logger.Trace("Skip to handle packet from {0}. Reason: client is no more connected.", this.RemoteEndPoint);
                return;
            }

            try
            {
                packet.Read<uint>(); // DPID: Always 0xFFFFFFFF

                pak = packet as FFPacket;
                packetHeaderNumber = packet.Read<uint>();

                if (Logger.IsTraceEnabled)
                    Logger.Trace("Received {0} packet from {1}.", (PacketType)packetHeaderNumber, this.RemoteEndPoint);

                PacketHandler<ClusterClient>.Invoke(this, pak, (PacketType)packetHeaderNumber);
            }
            catch (KeyNotFoundException)
            {
                if (Enum.IsDefined(typeof(PacketType), packetHeaderNumber))
                    Logger.Warn("Received an unimplemented Cluster packet {0} (0x{1}) from {2}.", Enum.GetName(typeof(PacketType), packetHeaderNumber), packetHeaderNumber.ToString("X4"), this.RemoteEndPoint);
                else
                    Logger.Warn("[SECURITY] Received an unknown Cluster packet 0x{0} from {1}.", packetHeaderNumber.ToString("X4"), this.RemoteEndPoint);
            }
            catch (RhisisPacketException packetException)
            {
                Logger.Error("Packet handle error from {0}. {1}", this.RemoteEndPoint, packetException);
                Logger.Debug(packetException.InnerException?.StackTrace);
            }
        }
    }
}
