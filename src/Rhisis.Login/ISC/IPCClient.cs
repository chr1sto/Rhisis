using Ether.Network;
using Ether.Network.Packets;
using Rhisis.Core.Exceptions;
using Rhisis.Core.IO;
using Rhisis.Core.ISC;
using Rhisis.Core.ISC.Packets;
using Rhisis.Core.ISC.Structures;
using Rhisis.Core.Network;
using Rhisis.Login.ISC.Packets;
using System;
using System.Collections.Generic;

namespace Rhisis.Login.ISC
{
    public sealed class ISCClient : NetConnection
    {
        private ISCServer _server;
        
        public InterServerType Type { get; private set; }

        public BaseServerInfo ServerInfo { get; private set; }

        public void Initialize(ISCServer server)
        {
            this._server = server;
            PacketFactory.SendWelcome(this);
        }

        public override void HandleMessage(NetPacketBase packet)
        {
            var packetHeaderNumber = packet.Read<uint>();

            try
            {
                PacketHandler<ISCClient>.Invoke(this, packet, (InterPacketType)packetHeaderNumber);
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

        public void Disconnect()
        {
            if (this.Type == InterServerType.Cluster)
            {
                var clusterInfo = this.ServerInfo as ClusterServerInfo;

                clusterInfo.Worlds.Clear();
            }
            else if (this.Type == InterServerType.World)
            {
                var worldInfo = this.GetServerInfo<WorldServerInfo>();
                ISCClient cluster = this._server.GetCluster(worldInfo.ParentClusterId);
                var clusterInfo = cluster?.GetServerInfo<ClusterServerInfo>();

                clusterInfo?.Worlds.Remove(worldInfo);
            }
        }

        public T GetServerInfo<T>() where T : BaseServerInfo
        {
            return this.ServerInfo as T;
        }

        [PacketHandler(InterPacketType.Authentication)]
        private void OnAuthenticate(NetPacketBase packet)
        {
            var id = packet.Read<int>();
            var host = packet.Read<string>();
            var name = packet.Read<string>();
            var type = packet.Read<byte>();
            this.Type = (InterServerType)type;

            if (this.Type == InterServerType.Cluster)
            {
                if (this._server.HasClusterWithId(id))
                {
                    Logger.Warning("Server '{0}' disconnected. Reason: Cluster already exists.", name);
                    PacketFactory.SendAuthenticationResult(this, InterServerError.AUTH_FAILED_CLUSTER_EXISTS);
                    this._server.DisconnectClient(this.Id);
                }

                this.ServerInfo = new ClusterServerInfo(id, host, name);

                //this._server.Clusters.Add(this.ServerInfo as ClusterServerInfo);
                PacketFactory.SendAuthenticationResult(this, InterServerError.AUTH_SUCCESS);
                Logger.Info("Cluster Server '{0}' connected to InterServer.", name);
            }
            else if (this.Type == InterServerType.World)
            {
                var clusterId = packet.Read<int>();
                // TODO: read more informations about world server if needed

                if (!this._server.HasClusterWithId(clusterId))
                {
                    // No cluster for this server
                    Logger.Warning("Cluster Server with id: '{0}' doesn't exists for World Server '{1}'", clusterId, name);
                    PacketFactory.SendAuthenticationResult(this, InterServerError.AUTH_FAILED_NO_CLUSTER);
                    this._server.DisconnectClient(this.Id);
                }

                ISCClient cluster = this._server.GetCluster(clusterId);
                var clusterInfo = cluster.GetServerInfo<ClusterServerInfo>();

                if (this._server.HasWorldInCluster(clusterId, id))
                {
                    // World already exists in cluster
                    Logger.Warning("World Server '{0}' already exists in Cluster '{1}'", name, clusterInfo.Name);
                    PacketFactory.SendAuthenticationResult(this, InterServerError.AUTH_FAILED_WORLD_EXISTS);
                    this._server.DisconnectClient(this.Id);
                }

                this.ServerInfo = new WorldServerInfo(id, host, name, clusterId);
                clusterInfo.Worlds.Add(this.ServerInfo as WorldServerInfo);
                PacketFactory.SendAuthenticationResult(this, InterServerError.AUTH_SUCCESS);
                Logger.Info("World Server '{0}' connected to Cluster '{1}'.", name, clusterInfo.Name);
            }
            else
            {
                PacketFactory.SendAuthenticationResult(this, InterServerError.AUTH_FAILED_UNKNOW_SERVER);
                this._server.DisconnectClient(this.Id);
            }
        }
    }
}
