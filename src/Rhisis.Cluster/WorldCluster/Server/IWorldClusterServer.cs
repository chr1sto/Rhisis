using Rhisis.Core.Structures.Configuration;
using Sylver.Network.Server;

namespace Rhisis.Cluster.WorldCluster.Server
{
    public interface IWorldClusterServer : INetServer
    {
        public WorldClusterConfiguration WorldClusterConfiguration { get; }
    }
}