using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Cluster.Packets;
using Rhisis.Core.Exceptions;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Core.ISC.Structures;
using Rhisis.Core.Network;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Structures.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Cluster
{
    public sealed class ClusterClient : NetConnection
    {
        private readonly uint _sessionId;
        private ClusterServer _clusterServer;

        /// <summary>
        /// Gets the cluster server's configuration.
        /// </summary>
        public ClusterConfiguration Configuration => this._clusterServer.ClusterConfiguration;

        /// <summary>
        /// Gets or sets the Login protect value.
        /// </summary>
        public int LoginProtectValue { get; set; }

        /// <summary>
        /// Creates a new <see cref="ClusterClient"/> instance.
        /// </summary>
        public ClusterClient()
        {
            this._sessionId = RandomHelper.GenerateSessionKey();
            this.LoginProtectValue = new Random().Next(0, 1000);
        }

        /// <summary>
        /// Initialize the <see cref="ClusterClient"/>.
        /// </summary>
        /// <param name="clusterServer"></param>
        public void InitializeClient(ClusterServer clusterServer)
        {
            this._clusterServer = clusterServer;
            CommonPacketFactory.SendWelcome(this, this._sessionId);
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
        /// Handle the incoming mesages.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public override void HandleMessage(NetPacketBase packet)
        {
            var pak = packet as FFPacket;
            var packetHeader = new PacketHeader(pak);

            if (!FFPacket.VerifyPacketHeader(packetHeader, (int)this._sessionId))
            {
                Logger.Warning("Invalid header for packet: {0}", packetHeader.Header);
                return;
            }

            packet.Read<uint>(); // DPID: Always 0xFFFFFFFF
            var packetHeaderNumber = packet.Read<uint>();

            try
            {
                PacketHandler<ClusterClient>.Invoke(this, pak, (PacketType)packetHeaderNumber);
            }
            catch (KeyNotFoundException)
            {
                FFPacket.UnknowPacket<PacketType>(packetHeaderNumber, 2);
            }
            catch (RhisisPacketException packetException)
            {
                Logger.Error(packetException.Message);
#if DEBUG
                Logger.Debug("STACK TRACE");
                Logger.Debug(packetException.InnerException?.StackTrace);
#endif
            }
        }
    }
}
