using System.Collections.Generic;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Network.Core;
using Sylver.Network.Server;

namespace Rhisis.Cluster.WorldCluster.Server
{
    public interface IWorldClusterServer : INetServer
    {
        public WorldClusterConfiguration WorldClusterConfiguration { get; }
    }
}