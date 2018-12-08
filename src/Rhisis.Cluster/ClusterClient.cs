using Ether.Network.Common;
using Ether.Network.Packets;
using Rhisis.Core.Exceptions;
using Rhisis.Core.Helpers;
using Rhisis.Network;
using Rhisis.Network.Packets;
using System;
using System.Collections.Generic;
using Rhisis.Core.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Rhisis.Cluster
{
    public sealed class ClusterClient : NetUser
    {
        private readonly ILogger<ClusterClient> _logger;
        private readonly IClusterServer _clusterServer;

        /// <summary>
        /// Gets the ID assigned to this session.
        /// </summary>
        public uint SessionId { get; }

        /// <summary>
        /// Gets or sets the Login protect value. 
        /// This value is random and valid only for this session in order to secure num pad disposition.
        /// </summary>
        public int LoginProtectValue { get; set; }

        /// <summary>
        /// Gets the remote end point (IP and port) for this client.
        /// </summary>
        public string RemoteEndPoint => this.Socket.RemoteEndPoint.ToString();

        /// <summary>
        /// Creates a new <see cref="ClusterClient"/> instance.
        /// </summary>
        public ClusterClient()
        {
            this.SessionId = RandomHelper.GenerateSessionKey();
            this.LoginProtectValue = new Random().Next(0, 1000);
            this._logger = DependencyContainer.Instance.Resolve<ILogger<ClusterClient>>();
            this._clusterServer = DependencyContainer.Instance.Resolve<IClusterServer>();
        }

        /// <summary>
        /// Disconnects the current <see cref="ClusterClient"/>.
        /// </summary>
        public void Disconnect()
        {
            this.Dispose();
            this._clusterServer.DisconnectClient(this.Id);
        }

        /// <summary>
        /// Send a packet to the client.
        /// </summary>
        /// <param name="packet"></param>
        public override void Send(INetPacketStream packet)
        {
            this._logger.LogTrace("Send {0} packet to {1}.",
                    (PacketType)BitConverter.ToUInt32(packet.Buffer, 5),
                    this.RemoteEndPoint);

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
                this._logger.LogTrace("Skip to handle packet from {0}. Reason: client is no more connected.", this.RemoteEndPoint);
                return;
            }

            try
            {
                packet.Read<uint>(); // DPID: Always 0xFFFFFFFF

                pak = packet as FFPacket;
                packetHeaderNumber = packet.Read<uint>();

                this._logger.LogTrace("Received {0} packet from {1}.", (PacketType)packetHeaderNumber, this.RemoteEndPoint);

                PacketHandler<ClusterClient>.Invoke(this, pak, (PacketType)packetHeaderNumber);
            }
            catch (KeyNotFoundException)
            {
                if (Enum.IsDefined(typeof(PacketType), packetHeaderNumber))
                    this._logger.LogWarning("Received an unimplemented Cluster packet {0} (0x{1}) from {2}.", Enum.GetName(typeof(PacketType), packetHeaderNumber), packetHeaderNumber.ToString("X4"), this.RemoteEndPoint);
                else
                    this._logger.LogWarning("[SECURITY] Received an unknown Cluster packet 0x{0} from {1}.", packetHeaderNumber.ToString("X4"), this.RemoteEndPoint);
            }
            catch (RhisisPacketException packetException)
            {
                this._logger.LogError("Packet handle error from {0}. {1}", this.RemoteEndPoint, packetException);
                this._logger.LogDebug(packetException.InnerException?.StackTrace);
            }
        }
    }
}
