using Rhisis.Core.Structures.Configuration;
using Rhisis.Network.Core;
using System.Collections.Generic;

namespace Rhisis.Cluster.CoreClient
{
    public interface IClusterCoreClient : ICoreClient
    {
        ClusterConfiguration ClusterConfiguration { get; }

        IList<WorldServerInfo> WorldServers { get; }
    }
}
