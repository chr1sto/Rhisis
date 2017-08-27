using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.Exceptions;
using Rhisis.Core.IO;
using Rhisis.Core.IPC;
using Rhisis.Core.IPC.Packets;
using Rhisis.Core.IPC.Structures;
using Rhisis.Core.Network;
using Rhisis.Login.IPC.Packets;
using System;
using System.Collections.Generic;

namespace Rhisis.Login.IPC
{
    public sealed class IPCClient : NetConnection
    {
        private IPCServer _server;

        public InterServerType Type { get; private set; }

        public BaseServerInfo ServerInfo { get; private set; }

        public void Initialize(IPCServer server)
        {
            this._server = server;
            PacketFactory.SendWelcome(this);
        }

        public override void HandleMessage(NetPacketBase packet)
        {
            var packetHeaderNumber = packet.Read<uint>();

            try
            {
                PacketHandler<IPCClient>.Invoke(this, packet, (InterPacketType)packetHeaderNumber);
            }
            catch (KeyNotFoundException)
            {
                Logger.Warning("Unknown inter-server packet with header: 0x{0}", packetHeaderNumber.ToString("X2"));
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

        [PacketHandler(InterPacketType.AUTHENTICATE)]
        private void OnAuthenticate(NetPacketBase packet)
        {
            var id = packet.Read<int>();
            var host = packet.Read<string>();
            var name = packet.Read<string>();
            var type = packet.Read<byte>();
            var interServerType = (InterServerType)type;

            this.ServerInfo = new BaseServerInfo(id, host, name);

            if (interServerType == InterServerType.Cluster)
            {
                if (this._server.HasClusterWithId(id))
                {
                    Logger.Warning("Server '{0}' disconnected. Reason: Cluster already exists.", name);
                    PacketFactory.SendAuthenticationResult(this, InterServerError.AUTH_FAILED_CLUSTER_EXISTS);
                    this._server.DisconnectClient(this.Id);
                }

                this._server.Clusters.Add(new ClusterServerInfo(id, host, name));
                PacketFactory.SendAuthenticationResult(this, InterServerError.AUTH_SUCCESS);
                Logger.Info("Cluster Server '{0}' connected to InterServer.", name);
            }
            else if (interServerType == InterServerType.World)
            {
                var clusterId = packet.Read<int>();
                // TODO: read more informations about world server

                if (!this._server.HasClusterWithId(clusterId))
                {
                    // No cluster for this server
                    Logger.Warning("Cluster Server with id: '{0}' doesn't exists for World Server '{1}'", clusterId, name);
                    PacketFactory.SendAuthenticationResult(this, InterServerError.AUTH_FAILED_NO_CLUSTER);
                    this._server.DisconnectClient(this.Id);
                }

                ClusterServerInfo cluster = this._server.GetCluster(clusterId);

                if (this._server.HasWorldInCluster(clusterId, id))
                {
                    // World already exists in cluster
                    Logger.Warning("World Server '{0}' already exists in Cluster '{1}'", name, cluster.Name);
                    PacketFactory.SendAuthenticationResult(this, InterServerError.AUTH_FAILED_WORLD_EXISTS);
                    this._server.DisconnectClient(this.Id);
                }
                
                cluster.Worlds.Add(new WorldServerInfo(id, host, name));
                PacketFactory.SendAuthenticationResult(this, InterServerError.AUTH_SUCCESS);
                Logger.Info("World Server '{0}' connected to Cluster '{1}'.", name, cluster.Name);
            }
            else
            {
                PacketFactory.SendAuthenticationResult(this, InterServerError.AUTH_FAILED_UNKNOW_SERVER);
                this._server.DisconnectClient(this.Id);
            }
        }
    }
}
