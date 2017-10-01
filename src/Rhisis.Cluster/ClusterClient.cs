using Ether.Network;
using System;
using Ether.Network.Packets;
using Rhisis.Cluster.Packets;
using Rhisis.Core.Network;
using Rhisis.Core.IO;
using Rhisis.Core.Exceptions;
using System.Collections.Generic;
using Rhisis.Core.Network.Packets;
using Rhisis.Core.Structures.Configuration;

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

        public ClusterClient()
        {
            this._sessionId = (uint)(new Random().Next(0, int.MaxValue));
        }

        public void InitializeClient(ClusterServer clusterServer)
        {
            this._clusterServer = clusterServer;
            ClusterPacketFactory.SendWelcome(this, this._sessionId);
        }

        public void Disconnect()
        {
            this.Dispose();
            this._clusterServer.DisconnectClient(this.Id);
        }

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
