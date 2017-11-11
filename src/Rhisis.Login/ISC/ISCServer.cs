using Ether.Network;
using Rhisis.Core.IO;
using Rhisis.Core.ISC;
using Rhisis.Core.ISC.Structures;
using Rhisis.Core.Network;
using Rhisis.Core.Structures.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Login.ISC
{
    public sealed class ISCServer : NetServer<ISCClient>
    {
        public IEnumerable<ClusterServerInfo> Clusters => from x in this.Clients
                                                          where x.ServerInfo is ClusterServerInfo
                                                          select x.ServerInfo as ClusterServerInfo;

        public ISCServer(ISCConfiguration configuration)
        {
            this.Configuration.Host = configuration.Host;
            this.Configuration.Port = configuration.Port;
            this.Configuration.MaximumNumberOfConnections = 100;
            this.Configuration.Backlog = 100;
            this.Configuration.BufferSize = 1024;
        }

        protected override void Initialize()
        {
            PacketHandler<ISCClient>.Initialize();
            Logger.Info("InterServer is up.");
        }

        protected override void OnClientConnected(ISCClient connection)
        {
            connection.Initialize(this);
        }

        protected override void OnClientDisconnected(ISCClient connection)
        {
            if (string.IsNullOrEmpty(connection.ServerInfo?.Name))
                Logger.Info("Unknow server disconnected from InterServer.");
            else
            {
                Logger.Info("Server '{0}' disconnected from InterServer.", connection.ServerInfo.Name);
                connection.Disconnect();
            }
        }

        internal bool HasClusterWithId(int id)
        {
            return this.Clients.Any(x => x.ServerInfo is ClusterServerInfo && x.ServerInfo.Id == id);
        }

        internal ISCClient GetCluster(int id)
        {
            return (from x in this.Clients
                    where x.ServerInfo is ClusterServerInfo
                    where x.ServerInfo.Id == id
                    select x).FirstOrDefault();
        }

        internal bool HasWorldInCluster(int clusterId, int worldId)
        {
            var cluster = this.GetCluster(clusterId);

            if (cluster == null)
                return false;

            var clusterInfo = cluster.GetServerInfo<ClusterServerInfo>();

            return clusterInfo.Worlds.Any(x => x.Id == worldId);
        }

        internal ISCClient GetWorld(int parentClusterId, int worldId)
        {
            return (from x in this.Clients
                    where x.Type == InterServerType.World
                    let worldInfo = x.ServerInfo as WorldServerInfo
                    where worldInfo.ParentClusterId == parentClusterId
                    where worldInfo.Id == worldId
                    select x).FirstOrDefault();
        }
    }
}
