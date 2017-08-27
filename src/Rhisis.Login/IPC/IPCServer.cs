using Ether.Network;
using Rhisis.Core.IO;
using Rhisis.Core.IPC.Structures;
using Rhisis.Core.Network;
using Rhisis.Core.Structures.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Login.IPC
{
    public sealed class IPCServer : NetServer<IPCClient>
    {
        public IList<ClusterServerInfo> Clusters { get; private set; }

        public IPCServer(IPCConfiguration configuration)
        {
            this.Clusters = new List<ClusterServerInfo>();
            this.Configuration.Host = configuration.Host;
            this.Configuration.Port = configuration.Port;
            this.Configuration.MaximumNumberOfConnections = 100;
            this.Configuration.Backlog = 100;
            this.Configuration.BufferSize = 1024;
        }

        protected override void Initialize()
        {
            PacketHandler<IPCClient>.Initialize();
            Logger.Info("InterServer is up.");
        }

        protected override void OnClientConnected(IPCClient connection)
        {
            connection.Initialize(this);
        }

        protected override void OnClientDisconnected(IPCClient connection)
        {
            if (string.IsNullOrEmpty(connection.ServerInfo?.Name))
                Logger.Info("Unknow server disconnected from InterServer.");
            else
            {
                Logger.Info("Server '{0}' disconnected from InterServer.", connection.ServerInfo.Name);
                this.Clusters.Remove(connection.ServerInfo as ClusterServerInfo);
            }
        }

        internal bool HasClusterWithId(int id)
        {
            return this.Clusters.Any(x => x.Id == id);
        }

        internal ClusterServerInfo GetCluster(int id)
        {
            return this.Clusters.FirstOrDefault(x => x.Id == id);
        }

        internal bool HasWorldInCluster(int clusterId, int worldId)
        {
            var cluster = this.GetCluster(clusterId);

            if (cluster == null)
                return false;

            return cluster.Worlds.Any(x => x.Id == worldId);
        }
    }
}
