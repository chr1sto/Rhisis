using Ether.Network;
using Rhisis.Core.IO;
using Rhisis.Core.ISC.Structures;
using Rhisis.Core.Structures.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Login.ISC
{
    public sealed class InterServer : NetServer<InterClient>
    {
        public IList<ClusterServerInfo> Clusters { get; private set; }

        public InterServer(InterServerConfiguration configuration)
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
            Logger.Info("Inter-Server is up.");
        }

        protected override void OnClientConnected(InterClient connection)
        {
            Logger.Info("A new server is connected to the InterServer.");
            connection.Initialize(this);
        }

        protected override void OnClientDisconnected(InterClient connection)
        {
            if (string.IsNullOrEmpty(connection.ServerInfo?.Name))
                Logger.Info("Unknow server disconnected from InterServer.");
            else
                Logger.Info("Server '{0}' disconnected from InterServer.", connection.ServerInfo.Name);
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
