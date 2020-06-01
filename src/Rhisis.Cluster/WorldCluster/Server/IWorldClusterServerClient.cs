using Rhisis.Network.Core;
using Sylver.Network.Common;

namespace Rhisis.Cluster.WorldCluster.Server
{
    public interface IWorldClusterServerClient : INetUser
    {
        public WorldServerInfo ServerInfo { get; set; }
    }
}