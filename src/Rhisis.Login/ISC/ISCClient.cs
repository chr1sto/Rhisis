using Ether.Network.Common;
using Ether.Network.Packets;
using NLog;
using Rhisis.Core.Exceptions;
using Rhisis.Core.ISC;
using Rhisis.Core.ISC.Packets;
using Rhisis.Core.ISC.Structures;
using Rhisis.Core.Network;
using Rhisis.Login.ISC.Packets;
using System;
using System.Collections.Generic;

namespace Rhisis.Login.ISC
{
    public sealed class ISCClient : NetUser
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private ISCServer _server;


        public ISCServer IcsServer => this._server;

        public ISCServerType Type { get; internal set; }

        public BaseServerInfo ServerInfo { get; internal set; }

        /// <summary>
        /// Gets the remote end point (IP and port).
        /// </summary>
        public string RemoteEndPoint { get; private set; }

        public void Initialize(ISCServer server)
        {
            this._server = server;
            this.RemoteEndPoint = this.Socket.RemoteEndPoint.ToString();
        }

        /// <inheritdoc />
        public override void Send(INetPacketStream packet)
        {
            if (Logger.IsTraceEnabled) 
            {
                Logger.Trace("Send {0} packet to {1}.", 
                    (ISCPacketType)BitConverter.ToUInt32(packet.Buffer, 4), 
                    this.RemoteEndPoint);
            }

            base.Send(packet);
        }

        /// <inheritdoc />
        public override void HandleMessage(INetPacketStream packet)
        {
            uint packetHeaderNumber = 0;

            if (Socket == null)
            {
                Logger.Trace("Skip to handle packet from {0}. Reason: socket is no more connected.", this.RemoteEndPoint);
                return;
            }

            try
            {
                packetHeaderNumber = packet.Read<uint>();

                if (Logger.IsTraceEnabled)
                    Logger.Trace("Received {0} packet from {1}.", (ISCPacketType)packetHeaderNumber, this.RemoteEndPoint);

                PacketHandler<ISCClient>.Invoke(this, packet, (ISCPacketType)packetHeaderNumber);
            }
            catch (KeyNotFoundException)
            {
                Logger.Warn("[SECURITY] Received an unknown ISC packet header 0x{0} from {1}.", 
                    packetHeaderNumber.ToString("X2"), this.RemoteEndPoint);
            }
            catch (RhisisPacketException packetException)
            {
                Logger.Error("ISC packet handle error from {0}. {1}", this.RemoteEndPoint, packetException);
                Logger.Debug(packetException.InnerException?.StackTrace);
            }
        }

        public void Disconnect()
        {
            switch (this.Type)
            {
                case ISCServerType.Cluster:
                    (this.ServerInfo as ClusterServerInfo)?.WorldServers.Clear();
                    break;
                case ISCServerType.World:
                    var worldInfo = this.GetServerInfo<WorldServerInfo>();
                    ISCClient cluster = this._server.GetCluster(worldInfo.ParentClusterId);
                    var clusterInfo = cluster?.GetServerInfo<ClusterServerInfo>();

                    if (clusterInfo == null)
                        return;

                    clusterInfo.WorldServers.Remove(worldInfo);
                    ISCPacketFactory.SendUpdateWorldList(cluster, clusterInfo.WorldServers);
                    break;
            }
        }

        public T GetServerInfo<T>() where T : BaseServerInfo
        {
            return this.ServerInfo as T;
        }
    }
}
