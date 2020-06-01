using Rhisis.Core.Structures.Configuration;
using Rhisis.Network.Core;

namespace Rhisis.Cluster.CoreClient
{
    /// <summary>
    /// Provides a mechanism to manage a cluster core client.
    /// </summary>
    public interface IClusterCoreClient : ICoreClient
    {
        /// <summary>
        /// Core configuration for core server to connect to
        /// </summary>
        CoreConfiguration CoreConfiguration { get; }
    }
}
